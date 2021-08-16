using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlimeAI : MonoBehaviour
{

    #region PUBLIC VARIABLES

    public SLIME_STATE states;
    //public Attack 
    public EnemyEntity entity;
    public EnemyPathfinding movementController;

    public float attackSpeed = 1;
    #endregion


    #region UNITY METHODS

    public void Awake()
    {
        states = SLIME_STATE.IDLE;
        entity = GetComponent<EnemyEntity>();
        movementController = GetComponent<EnemyPathfinding>();
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
        entity.normalizedHorizontalSpeed = 0;
        entity._animator.speed = 1;
        entity._animator.Play(Animator.StringToHash("Slime_Idle"));
        yield return new WaitForSeconds(0.5f);


        // EXECUTE IDLE STATE
        while (states == SLIME_STATE.IDLE)
        {
            //Check for target, using sight maybe?
            entity.SearchForTarget();

            if (entity.target != null)
            {
                states = SLIME_STATE.MOVETO;
            }
            yield return null;
        }

        // EXIT THE IDLE STATE

    }

    IEnumerator CHASE()
    {
        // ENTER THE CHASE STATE

        // EXECUTE CHASE STATE
        while (states == SLIME_STATE.CHASE)
        {



            yield return null;
        }

        // EXIT THE CHASE STATE

    }


    IEnumerator MOVETO()
    {

        movementController.MoveToTarget(entity.target);


        while (states == SLIME_STATE.MOVETO)
        {

            if (entity.target == null || movementController.mCurrentNodeId == -1)
            {
                movementController.mCurrentNodeId = -1;
                states = SLIME_STATE.IDLE;
                break;
            }
            movementController.JumperPathFollow();

            //If we caught up with the path
            if (movementController.mCurrentNodeId >= movementController.mPath.Count && !entity.knockedBack)
            {

                movementController.mCurrentNodeId = -1;
                states = SLIME_STATE.SLIMEATTACK1;
                break;
            }

            //If we are in range of target
            if (Vector3.Distance(entity.target.transform.position, transform.position) < 1.5 && !entity.knockedBack)
            {
                movementController.mCurrentNodeId = -1;
                states = SLIME_STATE.SLIMEATTACK1;
                break;
            }


            //
            movementController.MoveToTarget();



            yield return null;

        }






    }

    IEnumerator SLIMEATTACK1()
    {
        // ENTER THE ATTACK STATE
        entity.normalizedHorizontalSpeed = entity.GetDirection();

        //entity.normalizedHorizontalSpeed = entity.GetDirection();
        float speedMod = attackSpeed;
        //entity._velocity.y = Mathf.Sqrt(entity.maxJumpHeight * -GambleConstants.GRAVITY);
        //entity._velocity.x = entity.GetDirection()*entity.movementSpeed*2;
        entity.movementSpeed *= speedMod;


        // EXECUTE ATTACK STATE
        while (states == SLIME_STATE.SLIMEATTACK1)
        {
            entity._animator.Play(Animator.StringToHash("Slime_Attack"));
            entity._animator.speed = speedMod;

            if(!entity._animator.GetCurrentAnimatorStateInfo(0).IsName("Slime_Attack"))
            {
                yield return null;
            }

            //This checks if the animation has completed one cycle, and won't progress until it has
            //This allows for the animator speed to be adjusted by the "attack speed"
            while (entity._animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
            {
                yield return null;
            }

            entity.movementSpeed /= speedMod;
            entity._animator.speed = 1;
            entity._animator.Play(Animator.StringToHash("Slime_Idle"));
            Debug.Log("Setting to Idle from A1");
            states = SLIME_STATE.IDLE;
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
        while (states == SLIME_STATE.SLIMEATTACK2)
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

            states = SLIME_STATE.MOVETO;
            yield return null;
        }

        // EXIT THE ATTACK STATE

    }

    public void FireProjectile(Projectile projectile)
    {
        Projectile proj = Instantiate(projectile, transform.position, Quaternion.identity);
        proj._attackObject.SetOwner(entity);
        proj.SetDirection(GetAim());
    }

    //This is not possible to use as an animation event, can fix with scriptable object (that would hold our params
    public void FireProjectile(Projectile projectile, float angle)
    {
        Projectile proj = Instantiate(projectile, transform.position, Quaternion.identity);
        proj._attackObject.SetOwner(entity);
        proj.SetDirection(MathUtilities.DegreeToVector2(angle));
    }

    #endregion

    public Vector2 GetAim()
    {
        //If for some reason the slime attacks without a target, just shoot forward
        if(entity.target == null)
        {
            return entity.GetDirection() * Vector2.right;
        }

        //Otherwise, aim at target
        return (entity.target.transform.position - transform.position).normalized;
    }

}

#region enums

public enum SLIME_STATE
{
    IDLE = 0,
    CHASE = 1,
    SLIMEATTACK1 = 2,
    SLIMEATTACK2 = 3,
    MOVETO= 4
}

public enum AIM_TYPE
{

}

#endregion

