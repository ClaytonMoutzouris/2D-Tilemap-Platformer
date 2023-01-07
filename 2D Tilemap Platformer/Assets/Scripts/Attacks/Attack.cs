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
    public PlayerController entity;

    //A basic attack.
    public virtual IEnumerator Activate(PlayerController user, ButtonInput button = ButtonInput.LightAttack)
    {
        entity = user;
        StartUp();

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
        entity._animator.speed = 1;
        CleanUp();

    }

    public virtual void StartUp()
    {
        if(entity != null)
        {
            entity._attackManager.activeAttack = this;
        }
        if(entity.movementState != PlayerMovementState.Jump)
        {
            entity._controller.velocity = Vector3.zero;
        }

        entity.movementState = PlayerMovementState.Attacking;
        
    }

    public virtual void CleanUp()
    {
        entity._animator.speed = 1;
        entity._attackManager.activeAttack = null;
        entity.movementState = PlayerMovementState.Idle;

    }

}
