using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttack", menuName = "ScriptableObjects/Attacks/EnemyAttacks/EnemyAttack")]
public class EnemyAttack : ScriptableObject
{
    //for now assume one
    public AnimationClip[] attackAnimation;


    public float attackSpeed = 1;
    public AttackData attackData;
    public Entity attacker;

    public virtual void SetAttacker(PlayerController user)
    {
        attacker = user;
    }

    //A basic attack.
    public virtual IEnumerator Activate(ButtonInput button = ButtonInput.LightAttack)
    {
        StartUp();

        attacker._animator.speed = attackSpeed;
        attacker._animator.Play(attackAnimation[0].name);

        if (!attacker._animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation[0].name))
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

        //attacker._attackManager.activeAttack = this;

    }

    public virtual void CleanUp()
    {
        attacker._animator.speed = 1;
        //attacker._attackManager.activeAttack = null;

    }

}
