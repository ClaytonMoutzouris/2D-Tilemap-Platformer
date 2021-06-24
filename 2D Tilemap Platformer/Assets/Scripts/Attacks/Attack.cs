using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Attacks/Attack")]
public class Attack : ScriptableObject
{

    public AnimationClip attackAnimation;
    public AnimationClip attackAnimation2;

    public float attackSpeed = 1;
    public PlayerController entity;

    //A basic attack.
    public virtual IEnumerator Activate(PlayerController user)
    {
        entity = user;
        StartUp();
        //entity.overrideController["PlayerAttack1"] = ownerAnimation;
        entity._animator.Play(attackAnimation.name);
        entity._animator.speed = attackSpeed;

        float waitTime = attackAnimation.length * (1 / entity._animator.speed);

        //attackObject.ActivateObject();

        yield return new WaitForSeconds(waitTime);
        entity._animator.speed = 1;
        CleanUp();

    }

    public virtual void StartUp()
    {

    }

    public virtual void CleanUp()
    {
        entity._animator.speed = 1;
        entity._attackManager.activeAttack = null;
    }

}
