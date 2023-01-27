using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Attacks/Attack")]
public class Attack : ScriptableObject
{

    public AnimationClip attackAnimation;
    public AnimationClip attackAnimation2;
    public AnimationClip startupAnimation;
    public AnimationClip recoveryAnimation;

    public float attackSpeed = 1;

   [HideInInspector]
    public Entity attacker;

    public AttackData attackData;

    public virtual void SetAttacker(Entity user)
    {
        attacker = user;
    }

    //A basic attack.
    public virtual IEnumerator Activate(ButtonInput button = ButtonInput.LightAttack)
    {
        StartUp();

        attacker._animator.speed = attackSpeed;
        attacker._animator.Play(attackAnimation.name);

        if (!attacker._animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation.name))
        {
            yield return null;
        }

        //This checks if the animation has completed one cycle, and won't progress until it has
        //This allows for the animator speed to be adjusted by the "attack speed"
        while (attacker._animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {
            yield return null;
        }

        //float waitTime = attackAnimation.length * (1 / entity._animator.speed);
        //yield return new WaitForSeconds(waitTime);
        CleanUp();

    }

    public virtual void StartUp()
    {

  
    }

    public virtual void CleanUp()
    {
        attacker._animator.speed = 1;

    }

}
