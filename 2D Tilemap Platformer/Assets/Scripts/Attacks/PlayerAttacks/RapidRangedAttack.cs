using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RapidRangedAttack", menuName = "ScriptableObjects/Attacks/RapidRangedAttack")]
public class RapidRangedAttack : RangedAttack
{
    //move these to the parent class
    public float fireRate = 1; //currently in shots per second
    float lastFiredTimestamp = 0;

    //A basic attack.
    public override IEnumerator Activate(PlayerController user)
    {
        entity = user;
        StartUp();
        entity._animator.speed = attackSpeed;

        while (user._input.GetButton(ButtonInput.LightAttack))
        {
            entity._animator.Play(attackAnimation.name);

            if(Time.time > lastFiredTimestamp + (1/fireRate))
            {

                lastFiredTimestamp = Time.time;
                user._attackManager.FireProjectile();
            }
            yield return null;
        }



        entity._animator.speed = 1;
        entity._animator.Play(Animator.StringToHash("Idle"));
        CleanUp();
    }
}
