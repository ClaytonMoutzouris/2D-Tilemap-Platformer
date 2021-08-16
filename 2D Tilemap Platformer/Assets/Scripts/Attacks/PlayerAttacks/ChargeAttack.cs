using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChargeAttack", menuName = "ScriptableObjects/Attacks/ChargeAttack")]
public class ChargeAttack : Attack
{
    public float chargeMultiplier = 0;
    public float maxChargeMultiplier = 2.5f;
    public float chargeDuration = 2;
    public ParticleSystem chargingEffect;
    public ParticleSystem maxChargeEffect;
    public bool charged = false;

    //A basic attack.
    public override IEnumerator Activate(PlayerController user, ButtonInput button = ButtonInput.LightAttack)
    {
        entity = user;
        StartUp();
        float chargeTimestamp = Time.time;
        //float oldKnockback = entity._attackManager.meleeWeaponObject.knockbackPower;
        //int oldDamage = entity._attackManager.meleeWeaponObject.damage;
        chargeMultiplier = 0;

        //entity.movementState = PlayerMovementState.Attacking;
        entity._animator.Play(attackAnimation2.name);

        //entity.overrideController["PlayerAttack1"] = ownerAnimation;
        //entity._animator.speed = attackSpeed;
        ParticleSystem newEffect = user.AddEffect(chargingEffect);

        while (user._input.GetButton(button))
        {
            float percent =  Mathf.Clamp01(Mathf.Abs(Time.time - chargeTimestamp) / chargeDuration);
            chargeMultiplier = (maxChargeMultiplier-1)* percent + 1;

            if(percent == 1 && !charged)
            {
                charged = true;
                user.RemoveEffect(newEffect);
                newEffect = user.AddEffect(maxChargeEffect);
            }

            yield return null;
        }
        WeaponAttributeBonus damageBonus = null;
        WeaponAttributeBonus knockbackBonus = null;

        if (chargeMultiplier > 1)
        {
            chargeMultiplier -= 1;
            //These can be changed to now just update the player (or weapon?) stats.
            //entity._attackManager.meleeWeaponObject.knockbackPower = oldKnockback * chargeMultiplier;
            //entity._attackManager.meleeWeaponObject.damage = (int)(oldDamage * chargeMultiplier);
            knockbackBonus = new WeaponAttributeBonus(WeaponAttributesType.KnockbackPower, chargeMultiplier, StatModType.Mult);
            entity._equipmentManager.equippedWeapon.weaponAttributes.AddBonus(knockbackBonus);

            damageBonus = new WeaponAttributeBonus(WeaponAttributesType.Damage, chargeMultiplier, StatModType.Mult);
            entity._equipmentManager.equippedWeapon.weaponAttributes.AddBonus(damageBonus);
        }

        entity.RemoveEffect(newEffect);
        entity._animator.Play(attackAnimation.name);
        entity._animator.speed = attackSpeed;
        if (!entity._animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation.name))
        {
            yield return null;
        }

        //This checks if the animation has completed one cycle, and won't progress until it has
        //This allows for the animator speed to be adjusted by the "attack speed"
        while (entity._animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {
            yield return null;
        }

        //float waitTime = attackAnimation.length * (1 / entity._animator.speed);
        //yield return new WaitForSeconds(waitTime);
        //entity._attackManager.meleeWeaponObject.knockbackPower = oldKnockback;
        //entity._attackManager.meleeWeaponObject.damage = oldDamage;

        entity._equipmentManager.equippedWeapon.weaponAttributes.RemoveBonus(knockbackBonus);
        entity._equipmentManager.equippedWeapon.weaponAttributes.RemoveBonus(damageBonus);
        

        entity._animator.speed = 1;
        //entity.movementState = PlayerMovementState.Idle;
        CleanUp();
    }

}
