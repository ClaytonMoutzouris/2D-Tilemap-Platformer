using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerAttack", menuName = "ScriptableObjects/Attacks/PlayerAttacks/PlayerAttack")]
public class PlayerAttack : Attack
{
    [HideInInspector]
    public PlayerController player;

    public void SetPlayer(PlayerController player)
    {
        this.player = player;
    }

    public override void SetAttacker(Entity user)
    {
        base.SetAttacker(user);

        SetPlayer((PlayerController)user);
    }

    //A basic attack.
    public override IEnumerator Activate(ButtonInput button = ButtonInput.LightAttack)
    {
        StartUp();

        player._animator.speed = attackSpeed;
        player._animator.Play(attackAnimation.name);

        if (!player._animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation.name))
        {
            yield return null;
        }

        //This checks if the animation has completed one cycle, and won't progress until it has
        //This allows for the animator speed to be adjusted by the "attack speed"
        while (player._animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {
            yield return null;
        }

        //float waitTime = attackAnimation.length * (1 / entity._animator.speed);
        //yield return new WaitForSeconds(waitTime);
        CleanUp();

    }

    public override void StartUp()
    {
        base.StartUp();

        if (player != null)
        {
            player._attackManager.activeAttack = this;
        }

        if (player._controller.isGrounded)
        {
            player._controller.velocity = Vector3.zero;
        }

        player.movementState = PlayerMovementState.Attacking;

    }

    public override void CleanUp()
    {
        base.CleanUp();

        player._animator.speed = 1;
        player._attackManager.activeAttack = null;
        player.movementState = PlayerMovementState.Idle;

    }
}
