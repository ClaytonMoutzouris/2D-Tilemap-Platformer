using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PullEffect", menuName = "ScriptableObjects/Effects/PullEffect")]
public class PullEffect : TimedEffect
{
    public Chain chainPrefab;
    public float maxPullDistance = 20;
    public float pullSpeed = 5;
    public LayerMask layermask;
    Chain activeChain;

    public Vector3 pullDirection = Vector2.right;
    PlayerMovementState oldState;
    Vector3 pullDestination;

    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(owner, effected, data);

        if(effectedEntity)
        {
            activeChain = Instantiate(chainPrefab);
            activeChain.SetObjects(effectedEntity.gameObject, null);
            activeChain.endPos = pullDestination;

            if(effectedEntity is PlayerController player)
            {
                oldState = player.movementState;
                player.movementState = PlayerMovementState.GrapplingHooking;
            }

        }

    }

    public bool AttemptPull(Entity effected)
    {
        pullDirection.x = Mathf.Abs(pullDirection.x) * effected.GetDirection();

        if (effected is PlayerController player)
        {
            Vector2 tempDir = player._input.GetRightStickAim();
            if (tempDir != Vector2.zero)
            {
                pullDirection = tempDir;
            }

        }


        RaycastHit2D[] hits = Physics2D.RaycastAll(effected.transform.position, pullDirection.normalized, maxPullDistance, layermask);


        foreach (RaycastHit2D hit in hits)
        {
            Entity entity = hit.collider.GetComponent<Entity>();
            if (entity == effected)
            {
                continue;
            }

            pullDestination = hit.point;


            //_attackObject.hitbox.size = spriteRenderer.bounds.size;

            return true;
        }

        return false;
    }

    public override IEnumerator HandleEffect()
    {
        timeStamp = Time.time;

        while (unlimitedDuration || Time.time < timeStamp + duration)
        {
            if (!effectedEntity)
            {
                break;
            }

            if(effectedEntity._controller.collisionState.right && pullDirection.x > 0
                || effectedEntity._controller.collisionState.left && pullDirection.x < 0
                || effectedEntity._controller.collisionState.above && pullDirection.y > 0
                || effectedEntity._controller.collisionState.below && pullDirection.y < 0)
            {
                break;
            }

            Vector3 direction = (pullDestination - effectedEntity.transform.position).normalized;

            effectedEntity._controller.velocity = direction * pullSpeed;

            yield return null;
        }

        RemoveEffect();
    }

    public override void RemoveEffect()
    {
        if(activeChain)
        {
            Destroy(activeChain.gameObject);
        }

        if(effectedEntity is PlayerController player)
        {
            player.movementState = oldState;
        }

        base.RemoveEffect();
    }

    public override bool CheckRequirements(Entity owner, Entity effected)
    {
        bool canApply = base.CheckRequirements(owner, effected);

        if(canApply)
        {
            canApply = AttemptPull(effected);
        }

        return canApply;
    }
}
