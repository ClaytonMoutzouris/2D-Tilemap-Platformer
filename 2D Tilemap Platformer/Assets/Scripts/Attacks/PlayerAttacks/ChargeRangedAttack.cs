using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChargeRangedAttack", menuName = "ScriptableObjects/Attacks/ChargeRangedAttack")]
public class ChargeRangedAttack : Attack
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
        //float oldKnockback = entity._attackManager.rangedWeaponObject.knockbackPower;
        //int oldDamage = entity._attackManager.rangedWeaponObject.damage;

        chargeMultiplier = 0;

        //entity.movementState = PlayerMovementState.Attacking;
        entity._animator.Play(attackAnimation2.name);

        //entity.overrideController["PlayerAttack1"] = ownerAnimation;
        //entity._animator.speed = attackSpeed;
        ParticleSystem newEffect = user.AddEffect(chargingEffect);

        while (user._input.GetButton(button))
        {
            float percent = Mathf.Clamp01(Mathf.Abs(Time.time - chargeTimestamp) / chargeDuration);
            chargeMultiplier = (maxChargeMultiplier - 1) * percent + 1;

            if (percent == 1 && !charged)
            {
                charged = true;
                user.RemoveEffect(newEffect);
                newEffect = user.AddEffect(maxChargeEffect);
            }

            yield return null;
        }

        WeaponAttributeBonus damageBonus = null;


        if (chargeMultiplier > 1)
        {
            //entity._attackManager.rangedWeaponObject.knockbackPower = oldKnockback * chargeMultiplier;
            damageBonus = new WeaponAttributeBonus(WeaponAttributesType.Damage, chargeMultiplier, StatModType.Mult);
            entity._equipmentManager.equippedWeapon.weaponAttributes.AddBonus(damageBonus);
        }

        entity.RemoveEffect(newEffect);
        entity._animator.Play(attackAnimation.name);
        entity._animator.speed = attackSpeed * chargeMultiplier;
        float waitTime = attackAnimation.length * (1 / entity._animator.speed);

        //attackObject.ActivateObject();

        yield return new WaitForSeconds(waitTime);
        //entity._attackManager.rangedWeaponObject.knockbackPower = oldKnockback;
        entity._equipmentManager.equippedWeapon.weaponAttributes.RemoveBonus(damageBonus);

        entity._animator.speed = 1;
        //entity.movementState = PlayerMovementState.Idle;
        CleanUp();
    }

}
