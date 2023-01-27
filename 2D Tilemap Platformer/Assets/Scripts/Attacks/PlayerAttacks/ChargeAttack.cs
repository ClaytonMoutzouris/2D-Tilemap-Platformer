using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChargeAttack", menuName = "ScriptableObjects/Attacks/PlayerAttacks/WeaponAttacks/ChargeAttack")]
public class ChargeAttack : WeaponAttack
{
    public float chargeMultiplier = 0;
    public float maxChargeMultiplier = 2.5f;
    public float chargeDuration = 2;
    public ParticleSystem chargingEffect;
    public ParticleSystem maxChargeEffect;
    public bool charged = false;

    //A basic attack.
    public override IEnumerator Activate( ButtonInput button = ButtonInput.LightAttack)
    {
        StartUp();
        float chargeTimestamp = Time.time;
        //float oldKnockback = entity._attackManager.meleeWeaponObject.knockbackPower;
        //int oldDamage = entity._attackManager.meleeWeaponObject.damage;
        chargeMultiplier = 1;

        //entity.movementState = PlayerMovementState.Attacking;
        player._animator.Play(attackAnimation2.name);

        //entity.overrideController["PlayerAttack1"] = ownerAnimation;
        //entity._animator.speed = attackSpeed;
        ParticleSystem newEffect = player.AddEffect(chargingEffect);

        while (player._input.GetButton(button))
        {
            float percent =  Mathf.Clamp01(Mathf.Abs(Time.time - chargeTimestamp) / chargeDuration);
            chargeMultiplier = (maxChargeMultiplier-1) * percent + 1;

            if(percent == 1 && !charged)
            {
                charged = true;
                player.RemoveEffect(newEffect);
                newEffect = player.AddEffect(maxChargeEffect);
            }

            yield return null;
        }
        WeaponAttributeBonus damageBonus = null;
        WeaponAttributeBonus knockbackBonus = null;

        if (chargeMultiplier > 1)
        {
            //These can be changed to now just update the player (or weapon?) stats.
            //entity._attackManager.meleeWeaponObject.knockbackPower = oldKnockback * chargeMultiplier;
            //entity._attackManager.meleeWeaponObject.damage = (int)(oldDamage * chargeMultiplier);
            knockbackBonus = new WeaponAttributeBonus(WeaponAttributesType.KnockbackPower, chargeMultiplier, StatModType.Multiplier);
            weapon.weaponAttributes.AddBonus(knockbackBonus);

            damageBonus = new WeaponAttributeBonus(WeaponAttributesType.Damage, chargeMultiplier, StatModType.Multiplier);
            weapon.weaponAttributes.AddBonus(damageBonus);
        }

        player.RemoveEffect(newEffect);
        player._animator.Play(attackAnimation.name);
        player._animator.speed = attackSpeed * chargeMultiplier;

        if (!player._animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation.name))
        {
            //this waits to make sure the animation is set, in my experience this happens by the next frame when we get here
            yield return null;
        }

        //This checks if the animation has completed one cycle, and won't progress until it has
        //This allows for the animator speed to be adjusted by the "attack speed"
        while (player._animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {
            yield return null;
        }

        //float waitTime = attackAnimation.length * (1 / entity._animator.speed);
        //yield return new WaitForSeconds(waitTime);
        //entity._attackManager.meleeWeaponObject.knockbackPower = oldKnockback;
        //entity._attackManager.meleeWeaponObject.damage = oldDamage;

        weapon.weaponAttributes.RemoveBonus(knockbackBonus);
        weapon.weaponAttributes.RemoveBonus(damageBonus);
        

        player._animator.speed = 1;
        //entity.movementState = PlayerMovementState.Idle;
        CleanUp();
    }

}
