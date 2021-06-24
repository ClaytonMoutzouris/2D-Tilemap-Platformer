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
    public override IEnumerator Activate(PlayerController user)
    {
        entity = user;
        StartUp();
        float chargeTimestamp = Time.time;
        float oldKnockback = entity._attackManager.meleeWeaponObject.knockbackPower;
        int oldDamage = entity._attackManager.meleeWeaponObject.damage;

        chargeMultiplier = 0;

        //entity.movementState = PlayerMovementState.Attacking;
        entity._animator.Play(attackAnimation2.name);

        //entity.overrideController["PlayerAttack1"] = ownerAnimation;
        //entity._animator.speed = attackSpeed;
        ParticleSystem newEffect = user.AddEffect(chargingEffect);

        while (user._input.GetButton(ButtonInput.LightAttack))
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

        if (chargeMultiplier > 1)
        {
            entity._attackManager.meleeWeaponObject.knockbackPower = oldKnockback * chargeMultiplier;
            entity._attackManager.meleeWeaponObject.damage = (int)(oldDamage * chargeMultiplier);
        }

        entity.RemoveEffect(newEffect);
        entity._animator.Play(attackAnimation.name);
        entity._animator.speed = attackSpeed*chargeMultiplier;
        float waitTime = attackAnimation.length * (1 / entity._animator.speed);

        //attackObject.ActivateObject();

        yield return new WaitForSeconds(waitTime);
        entity._attackManager.meleeWeaponObject.knockbackPower = oldKnockback;
        entity._attackManager.meleeWeaponObject.damage = oldDamage;

        entity._animator.speed = 1;
        //entity.movementState = PlayerMovementState.Idle;
        CleanUp();
    }

}
