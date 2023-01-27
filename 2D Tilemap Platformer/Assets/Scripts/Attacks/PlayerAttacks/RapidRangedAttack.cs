using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RapidRangedAttack", menuName = "ScriptableObjects/Attacks/WeaponAttacks/RapidRangedAttack")]
public class RapidRangedAttack : RangedAttack
{
    //move these to the parent class
    float lastFiredTimestamp = 0;

    //A basic attack.
    public override IEnumerator Activate(ButtonInput button = ButtonInput.Fire)
    {
        StartUp();
        player._animator.speed = attackSpeed;
        lastFiredTimestamp = Time.time;

        while (player._input.GetButton(button))
        {
            player._animator.Play(attackAnimation.name);
            //user._attackManager.rangedWeaponObject.
            weapon.FireAimedProjectile();

            yield return null;
        }



        player._animator.speed = 1;
        //entity._animator.Play(Animator.StringToHash("Idle"));
        CleanUp();
    }
}
