using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    public bool isActive = false;
    public List<AttackObject> objectPrototypes;
    public AnimationClip ownerAnimation;
    public float attackSpeed = 1;
    public PlayerController player;
    //public List<AttackObject> activeObjects;

    public void SetObject(AttackObject attackObject)
    {
        objectPrototypes[0] = attackObject;
    }

    public void SetObject(AttackObject attackObject, int index)
    {
        objectPrototypes[index] = attackObject;
    }
    //A basic attack.
    public virtual IEnumerator Activate(PlayerController user)
    {
        player = user;
        StartUp();
        player.overrideController["PlayerAttack1"] = ownerAnimation;
        player._animator.Play(Animator.StringToHash("DEFAULT_ATTACK"));
        player._animator.speed = attackSpeed;

        foreach(AttackObject attackObj in objectPrototypes)
        {
            AttackObject temp = AttackObject.Instantiate(attackObj, transform);
            temp.animator.speed = attackSpeed;
            float duration = temp.animator.GetCurrentAnimatorStateInfo(0).length * (1 / temp.animator.speed);
            temp.ActivateObject(duration);
        }

        float waitTime = ownerAnimation.length * (1 / player._animator.speed);

        yield return new WaitForSeconds(waitTime);
        player._animator.speed = 1;
        player._animator.Play(Animator.StringToHash("Idle"));
        CleanUp();
    }

    public virtual void StartUp()
    {
        isActive = true;

    }

    public virtual void CleanUp()
    {
        isActive = false;
        Destroy(gameObject);

    }

}
