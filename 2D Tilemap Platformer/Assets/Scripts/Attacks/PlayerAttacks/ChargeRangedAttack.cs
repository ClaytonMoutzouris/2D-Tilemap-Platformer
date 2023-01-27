using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChargeRangedAttack", menuName = "ScriptableObjects/Attacks/PlayerAttacks/WeaponAttacks/ChargeRangedAttack")]
public class ChargeRangedAttack : WeaponAttack
{
    public float chargeMultiplier = 0;
    public float maxChargeMultiplier = 2.5f;
    public float chargeDuration = 2;
    public ParticleSystem chargingEffect;
    public ParticleSystem maxChargeEffect;
    public bool charged = false;

    //A basic attack.
    public override IEnumerator Activate(ButtonInput button = ButtonInput.LightAttack)
    {
        StartUp();
        float chargeTimestamp = Time.time;
        //float oldKnockback = entity._attackManager.rangedWeaponObject.knockbackPower;
        //int oldDamage = entity._attackManager.rangedWeaponObject.damage;

        chargeMultiplier = 0;

        //entity.movementState = PlayerMovementState.Attacking;
        player._animator.Play(attackAnimation2.name);

        //entity.overrideController["PlayerAttack1"] = ownerAnimation;
        //entity._animator.speed = attackSpeed;
        ParticleSystem newEffect = player.AddEffect(chargingEffect);

        while (player._input.GetButton(button))
        {
            float percent = Mathf.Clamp01(Mathf.Abs(Time.time - chargeTimestamp) / chargeDuration);
            chargeMultiplier = (maxChargeMultiplier - 1) * percent + 1;

            if (percent == 1 && !charged)
            {
                charged = true;
                player.RemoveEffect(newEffect);
                newEffect = player.AddEffect(maxChargeEffect);
            }

            yield return null;
        }

        WeaponAttributeBonus damageBonus = null;


        if (chargeMultiplier > 1)
        {
            //entity._attackManager.rangedWeaponObject.knockbackPower = oldKnockback * chargeMultiplier;
            damageBonus = new WeaponAttributeBonus(WeaponAttributesType.Damage, chargeMultiplier, StatModType.Multiplier);
            weapon.weaponAttributes.AddBonus(damageBonus);
        }

        player.RemoveEffect(newEffect);
        player._animator.Play(attackAnimation.name);
        player._animator.speed = attackSpeed * chargeMultiplier;
        float waitTime = attackAnimation.length * (1 / player._animator.speed);

        //attackObject.ActivateObject();

        yield return new WaitForSeconds(waitTime);
        //entity._attackManager.rangedWeaponObject.knockbackPower = oldKnockback;
        weapon.weaponAttributes.RemoveBonus(damageBonus);

        player._animator.speed = 1;
        //entity.movementState = PlayerMovementState.Idle;
        CleanUp();
    }

}
