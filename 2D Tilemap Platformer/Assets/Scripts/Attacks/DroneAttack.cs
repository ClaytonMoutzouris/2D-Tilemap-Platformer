using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DroneAttack", menuName = "ScriptableObjects/Attacks/DroneAttacks/DroneAttack")]
public class DroneAttack : Attack
{
    public Drone drone;

    public void SetDrone(Drone drone)
    {
        this.drone = drone;
    }

    public override IEnumerator Activate(ButtonInput button = ButtonInput.LightAttack)
    {
        StartUp();

        drone._animator.speed = attackSpeed;
        //drone._animator.Play(attackAnimation.name);

        /*
        if (!drone._animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation.name))
        {
            yield return null;
        }

        //This checks if the animation has completed one cycle, and won't progress until it has
        //This allows for the animator speed to be adjusted by the "attack speed"
        while (drone._animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {
            yield return null;
        }
        */
        drone.FireProjectile();
        yield return null;


        //float waitTime = attackAnimation.length * (1 / entity._animator.speed);
        //yield return new WaitForSeconds(waitTime);
        CleanUp();

    }

    public override void StartUp()
    {
        base.StartUp();


    }

    public override void CleanUp()
    {
        base.CleanUp();

    }
}
