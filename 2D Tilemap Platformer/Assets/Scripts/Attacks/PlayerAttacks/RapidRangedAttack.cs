using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RapidRangedAttack", menuName = "ScriptableObjects/Attacks/RapidRangedAttack")]
public class RapidRangedAttack : RangedAttack
{
    //move these to the parent class
    float lastFiredTimestamp = 0;

    //A basic attack.
    public override IEnumerator Activate(PlayerController user, ButtonInput button = ButtonInput.Fire)
    {
        entity = user;
        StartUp();
        entity._animator.speed = attackSpeed;
        lastFiredTimestamp = Time.time;

        while (user._input.GetButton(button))
        {
            entity._animator.Play(attackAnimation.name);
            //user._attackManager.rangedWeaponObject.
            user._equipmentManager.equippedWeapon.FireAimedProjectile();

            yield return null;
        }



        entity._animator.speed = 1;
        //entity._animator.Play(Animator.StringToHash("Idle"));
        CleanUp();
    }
}
