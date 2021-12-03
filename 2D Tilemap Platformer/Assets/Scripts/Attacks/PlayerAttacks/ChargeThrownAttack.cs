using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChargeThrownAttack", menuName = "ScriptableObjects/Attacks/ChargeThrownAttack")]
public class ChargeThrownAttack : Attack
{
    public float chargeMultiplier = 0;
    public float maxChargeMultiplier = 2.5f;
    public float chargeDuration = 1;
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

        WeaponAttributeBonus projectileSpeedBonus = null;


        if (chargeMultiplier > 1)
        {
            chargeMultiplier -= 1;
            //entity._attackManager.rangedWeaponObject.knockbackPower = oldKnockback * chargeMultiplier;
            //damageBonus = new WeaponAttributeBonus(WeaponAttributesType.Damage, chargeMultiplier, StatModType.Mult);
            projectileSpeedBonus = new WeaponAttributeBonus(WeaponAttributesType.ProjectileSpeed, chargeMultiplier, StatModType.Mult);
            
            entity._equipmentManager.equippedWeapon.weaponAttributes.AddBonus(projectileSpeedBonus);
        }

        entity.RemoveEffect(newEffect);
        entity._animator.Play(attackAnimation.name);
        entity._animator.speed = attackSpeed;
        float waitTime = attackAnimation.length * (1 / entity._animator.speed);

        //attackObject.ActivateObject();

        yield return new WaitForSeconds(waitTime);
        //entity._attackManager.rangedWeaponObject.knockbackPower = oldKnockback;
        entity._equipmentManager.equippedWeapon.weaponAttributes.RemoveBonus(projectileSpeedBonus);

        entity._animator.speed = 1;
        //entity.movementState = PlayerMovementState.Idle;
        CleanUp();
    }

}
