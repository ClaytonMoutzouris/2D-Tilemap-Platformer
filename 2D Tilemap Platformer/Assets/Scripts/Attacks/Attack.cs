using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    public bool isActive = false;
    public AnimationClip attackAnimation;
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
        isActive = true;

    }

    public virtual void CleanUp()
    {
        entity._animator.speed = 1;
        isActive = false;
        Destroy(gameObject);

    }

}
