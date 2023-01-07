using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{

    #region PUBLIC VARIABLES

    public ENEMY_STATE states;
    //public Attack 
    public EnemyEntity entity;
    public float attackSpeed = 1;

    #endregion


    #region UNITY METHODS

    public void Awake()
    {
        states = ENEMY_STATE.IDLE;
        entity = GetComponent<EnemyEntity>();
    }

    public void Start()
    {
        StartCoroutine(EnemyFSM());
    }

    #endregion

    #region ENEMY COROUTINES

    IEnumerator EnemyFSM()
    {
        while (true)
        {
            yield return StartCoroutine(states.ToString());
        }
    }

    IEnumerator IDLE()
    {
        // ENTER THE IDLE STATE

        // EXECUTE IDLE STATE
        while (states == ENEMY_STATE.IDLE)
        {
            entity.normalizedHorizontalSpeed = 0;

            //Check for target, using sight maybe?
            entity.SearchForTarget();

            if (entity.target != null)
            {
                states = ENEMY_STATE.CHASE;
            }
            yield return null;
        }

        // EXIT THE IDLE STATE

    }

    IEnumerator CHASE()
    {
        // ENTER THE CHASE STATE

        // EXECUTE CHASE STATE
        while (states == ENEMY_STATE.CHASE)
        {

            entity.normalizedHorizontalSpeed = 0;

            if (entity.target.transform.position.x - transform.position.x > 0)
            {
                entity.normalizedHorizontalSpeed = 1;

                if (transform.localScale.x < 0f)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else if (entity.target.transform.position.x - transform.position.x < 0)
            {
                entity.normalizedHorizontalSpeed = -1;
                if (transform.localScale.x > 0f)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }


            if (entity._controller.collisionState.left || entity._controller.collisionState.right || (entity.target.transform.position.y - transform.position.y) > 2.5)
            {
                Jump(entity.maxJumpHeight);

            }

            if(Vector3.Distance(entity.target.transform.position, transform.position) < 1.5)
            {
                states = ENEMY_STATE.SLIMEATTACK1;
            }
            else if (Vector3.Distance(entity.target.transform.position, transform.position) > 4 && Vector3.Distance(entity.target.transform.position, transform.position) < 6)
            {
                states = ENEMY_STATE.SLIMEATTACK2;
            }

            yield return null;
        }

        // EXIT THE CHASE STATE

    }

    IEnumerator SLIMEATTACK1()
    {
        // ENTER THE ATTACK STATE
        //entity.normalizedHorizontalSpeed = entity.GetDirection();
        entity._controller.velocity.y = Mathf.Sqrt(entity.maxJumpHeight/2 * -GambleConstants.GRAVITY);
        //entity._velocity.x = entity.GetDirection();

        // EXECUTE ATTACK STATE
        while (states == ENEMY_STATE.SLIMEATTACK1)
        {
            entity._animator.Play(Animator.StringToHash("Slime_Attack"));
            entity._animator.speed = attackSpeed;
            //attack.Activate(entity);

            //This checks if the animation has completed one cycle, and won't progress until it has
            //This allows for the animator speed to be adjusted by the "attack speed"
            while (entity._animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
            {
                yield return null;
            }

            entity._animator.speed = 1;
            entity._animator.Play(Animator.StringToHash("Slime_Idle"));

            states = ENEMY_STATE.IDLE;
            yield return null;
        }

        // EXIT THE ATTACK STATE

    }

    IEnumerator SLIMEATTACK2()
    {
        // ENTER THE ATTACK STATE
        entity.normalizedHorizontalSpeed = 0;
        //entity._velocity.y = Mathf.Sqrt(entity.jumpHeight / 2 * -GambleConstants.GRAVITY);
        //entity._velocity.x = entity.GetDirection();

        // EXECUTE ATTACK STATE
        while (states == ENEMY_STATE.SLIMEATTACK2)
        {
            entity._animator.Play(Animator.StringToHash("Slime_Attack2"));
            entity._animator.speed = attackSpeed;
            //attack.Activate(entity);
            //float waitTime = entity._animator.GetCurrentAnimatorStateInfo(0).length;

            //This checks if the animation has completed one cycle, and won't progress until it has
            //This allows for the animator speed to be adjusted by the "attack speed"
            while (entity._animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
            {
                yield return null;
            }
            //yield return new WaitForSeconds(waitTime);
            entity._animator.speed = 1;
            entity._animator.Play(Animator.StringToHash("Slime_Idle"));

            states = ENEMY_STATE.IDLE;
            yield return null;
        }

        // EXIT THE ATTACK STATE

    }

    public void FireProjectile(Projectile projectile)
    {
        Projectile proj = Instantiate(projectile, transform.position, Quaternion.identity);
        proj._attackObject.SetOwner(entity);
        proj.SetDirection(entity.GetDirection() * Vector2.right);
    }

    #endregion

    public void Jump(float jumpHeight)
    {
        if (!entity._controller.collisionState.below)
        {
            //Can't jump while jumping or in the air
            return;
        }

        if(jumpHeight > entity.maxJumpHeight)
        {
            jumpHeight = entity.maxJumpHeight;
        }

        entity._controller.velocity.y = Mathf.Sqrt(2 * jumpHeight * -GambleConstants.GRAVITY);

    }

}

#region enums

public enum ENEMY_STATE
{
    IDLE = 0,
    CHASE = 1,
    SLIMEATTACK1 = 2,
    SLIMEATTACK2 = 3
}

#endregion

