using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlimeAI : MonoBehaviour
{

    #region PUBLIC VARIABLES

    public SLIME_STATE states;
    //public Attack 
    public EnemyEntity entity;
    public Entity target;

    public float attackSpeed = 1;
    #endregion

    #region Pathfinding VARIABLES

    public List<Vector2Int> mPath = new List<Vector2Int>();
    public int mCurrentNodeId = -1;
    Vector2 mDestination;
    [SerializeField]
    float mFramesOfJumping = 0;
    [SerializeField]
    int mStuckFrames = 0;
    [SerializeField]
    int cMaxStuckFrames = 20;

    public Vector2 currentDest;
    public Vector2 prevDest;
    public Vector2 nextDest;

    public float mWidth = 1;
    public float mHeight = 3;
    public float cBotMaxPositionError = 1f;
    #endregion


    #region UNITY METHODS

    public void Awake()
    {
        states = SLIME_STATE.IDLE;
        entity = GetComponent<EnemyEntity>();
    }

    public void Start()
    {
        StartCoroutine(EnemyFSM());
        mWidth = entity._controller.boxCollider.size.x;
        mHeight = entity._controller.boxCollider.size.y;

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

        // EXECUTE IDLE STATE
        while (states == SLIME_STATE.IDLE)
        {
            //Check for target, using sight maybe?

            if (target != null)
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

            entity.normalizedHorizontalSpeed = 0;

            if (target.transform.position.x - transform.position.x > 0)
            {
                entity.normalizedHorizontalSpeed = 1;

                if (transform.localScale.x < 0f)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else if (target.transform.position.x - transform.position.x < 0)
            {
                entity.normalizedHorizontalSpeed = -1;
                if (transform.localScale.x > 0f)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }


            if (entity._controller.collisionState.left || entity._controller.collisionState.right || (target.transform.position.y - transform.position.y) > 2.5)
            {
                Jump(entity.maxJumpHeight);

            }

            if (Vector3.Distance(target.transform.position, transform.position) < 1.5)
            {
                states = SLIME_STATE.SLIMEATTACK1;
            }
            else if (Vector3.Distance(target.transform.position, transform.position) > 4 && Vector3.Distance(target.transform.position, transform.position) < 6)
            {
                states = SLIME_STATE.SLIMEATTACK2;
            }

            yield return null;
        }

        // EXIT THE CHASE STATE

    }

    IEnumerator MOVETO()
    {

        MoveTo(target.transform.position);


        while (states == SLIME_STATE.MOVETO)
        {

            int tileX, tileY;
            tileX = (int)transform.position.x;
            tileY = (int)transform.position.y;

            bool destOnGround, reachedY, reachedX;
            GetContext(out destOnGround, out reachedX, out reachedY);
            Vector2 pathPosition = transform.position;

            if (pathPosition.y - currentDest.y > cBotMaxPositionError && entity._controller.collisionState.onOneWayPlatform)
            {
                //drop down
                Debug.Log("Drop down");
                entity._controller.ignoreOneWayPlatformsThisFrame = true;
            }
            //mInputs[(int)KeyInput.GoDown] = true;



            if (reachedX && reachedY)
            {
                int prevNodeId = mCurrentNodeId;
                mCurrentNodeId++;

                if (mCurrentNodeId >= mPath.Count)
                {
                    mCurrentNodeId = -1;
                    states = SLIME_STATE.IDLE;
                    break;
                }

            }
            else if (!reachedX)
            {
                if (currentDest.x - pathPosition.x > cBotMaxPositionError)
                {
                    entity.normalizedHorizontalSpeed = 1;
                    if (transform.localScale.x < 0f)
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                else if (pathPosition.x - currentDest.x > cBotMaxPositionError)
                {
                    entity.normalizedHorizontalSpeed = -1;
                    if (transform.localScale.x > 0f)
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
            }
            else if (!reachedY && mPath.Count > mCurrentNodeId + 1 && !destOnGround)
            {
                float checkedX = 0;

                if (mPath[mCurrentNodeId + 1].x != mPath[mCurrentNodeId].x)
                {

                    if (mPath[mCurrentNodeId + 1].x > mPath[mCurrentNodeId].x)
                        checkedX = tileX + mWidth;
                    else
                        checkedX = tileX - 1;
                }

                if (checkedX != 0 && !GameGrid.instance.AnySolidBlockInStripe((int)checkedX, tileY, mPath[mCurrentNodeId + 1].y))
                {
                    if (nextDest.x - pathPosition.x > cBotMaxPositionError)
                    {
                        entity.normalizedHorizontalSpeed = 1;
                        if (transform.localScale.x < 0f)
                            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

                    }
                    else if (pathPosition.x - nextDest.x > cBotMaxPositionError)
                    {
                        entity.normalizedHorizontalSpeed = -1;
                        if (transform.localScale.x > 0f)
                            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

                    }

                    if (ReachedNodeOnXAxis(pathPosition, currentDest, nextDest) && ReachedNodeOnYAxis(pathPosition, currentDest, nextDest))
                    {
                        mCurrentNodeId += 1;
                    }
                }
            }

            if (Vector3.Distance(target.transform.position, transform.position) < 1.5)
            {
                mCurrentNodeId = -1;
                states = SLIME_STATE.SLIMEATTACK1;
                break;
            }

            if (entity._controller.isGrounded)
            {
                //Change this to just tell us if we need to jump or not
                mFramesOfJumping = GetJumpHeightForNode(mCurrentNodeId-1);

                if (mFramesOfJumping > 0)
                {
                    Jump(mFramesOfJumping);

                }
                 
            }

            if (Vector2.Distance(transform.position, entity.mOldPosition) == 0)
            {
                ++mStuckFrames;
                if (mStuckFrames > cMaxStuckFrames)
                {
                    Debug.Log("Slime got stuck");
                    MoveTo(mPath[mPath.Count - 1]);

                }
            }
            else
            {
                mStuckFrames = 0;
            }


            MoveTo(target.transform.position);

            yield return null;

        }






    }

    IEnumerator SLIMEATTACK1()
    {
        // ENTER THE ATTACK STATE
        entity.normalizedHorizontalSpeed = entity.GetDirection();

        //entity.normalizedHorizontalSpeed = entity.GetDirection();

        //entity._velocity.y = Mathf.Sqrt(entity.maxJumpHeight * -GambleConstants.GRAVITY);
        //entity._velocity.x = entity.GetDirection()*entity.movementSpeed*2;
        entity.movementSpeed *= 2;


        // EXECUTE ATTACK STATE
        while (states == SLIME_STATE.SLIMEATTACK1)
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
            entity.movementSpeed /= 2;
            entity._animator.speed = 1;
            entity._animator.Play(Animator.StringToHash("Slime_Idle"));

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

            states = SLIME_STATE.IDLE;
            yield return null;
        }

        // EXIT THE ATTACK STATE

    }

    public void FireProjectile(Projectile projectile)
    {
        Projectile proj = Instantiate(projectile, transform.position, Quaternion.identity);
        proj.SetDirection(GetAim());
    }

    //This is not possible to use as an animation event, can fix with scriptable object (that would hold our params
    public void FireProjectile(Projectile projectile, float angle)
    {
        Projectile proj = Instantiate(projectile, transform.position, Quaternion.identity);
        proj.SetDirection(MathUtilities.DegreeToVector2(angle));
    }

    #endregion

    public void Jump(float jumpHeight)
    {
        if (!entity._controller.isGrounded)
        {
            //Can't jump while jumping or in the air
            return;
        }

        Debug.Log("Jumping at Height: " + jumpHeight);

        if (jumpHeight > entity.maxJumpHeight)
        {
            jumpHeight = entity.maxJumpHeight;
        }
        //Basically just set the velocity to the jump speed
        entity._velocity.y = Mathf.Sqrt(2 * (jumpHeight + 0.5f) * -GambleConstants.GRAVITY);

    }

    public Vector2 GetAim()
    {
        //If for some reason the slime attacks without a target, just shoot forward
        if(target == null)
        {
            return entity.GetDirection() * Vector2.right;
        }

        //Otherwise, aim at target
        return (target.transform.position - transform.position).normalized;
    }

    public void GetContext(out bool destOnGround, out bool reachedX, out bool reachedY)
    {
        prevDest = new Vector2(mPath[mCurrentNodeId - 1].x + 0.5f,
                                             mPath[mCurrentNodeId - 1].y + 0.5f);
        currentDest = new Vector2(mPath[mCurrentNodeId].x + 0.5f,
                                          mPath[mCurrentNodeId].y + 0.5f);
        nextDest = currentDest;

        if (mPath.Count > mCurrentNodeId + 1)
        {
            nextDest = new Vector2(mPath[mCurrentNodeId + 1].x + 0.5f,
                                          mPath[mCurrentNodeId + 1].y + 0.5f);
        }

        destOnGround = false;
        for (int x = mPath[mCurrentNodeId].x; x < mPath[mCurrentNodeId].x + mWidth; ++x)
        {
            if (GameGrid.instance.IsGround(x, mPath[mCurrentNodeId].y - 1))
            {
                destOnGround = true;
                break;
            }
        }

        Vector2 pathPosition = transform.position;

        reachedX = ReachedNodeOnXAxis(pathPosition, prevDest, currentDest);
        reachedY = ReachedNodeOnYAxis(pathPosition, prevDest, currentDest);
        
        /*
        //snap the character if it reached the goal but overshot it by more than cBotMaxPositionError
        if (reachedX && Mathf.Abs(pathPosition.x - currentDest.x) > cBotMaxPositionError && Mathf.Abs(pathPosition.x - currentDest.x) < cBotMaxPositionError * 3.0f
            && entity.normalizedHorizontalSpeed != -1 && entity.normalizedHorizontalSpeed != 1)
        {
            pathPosition.x = currentDest.x;
            transform.position = new Vector3(pathPosition.x, transform.position.y, transform.position.z);
        }
        */

        if (destOnGround && !entity._controller.isGrounded)
            reachedY = false;

        if(reachedY)
        {
            //Debug.Log("Reach Y for " + currentDest);
        }

        if(reachedX)
        {
            //Debug.Log("Reached X for " + currentDest);
        }
    }

    public bool ReachedNodeOnXAxis(Vector2 pathPosition, Vector2 prevDest, Vector2 currentDest)
    {
        return (prevDest.x <= currentDest.x && pathPosition.x >= currentDest.x)
            || (prevDest.x >= currentDest.x && pathPosition.x <= currentDest.x)
            || Mathf.Abs(pathPosition.x - currentDest.x) <= cBotMaxPositionError;
    }

    public bool ReachedNodeOnYAxis(Vector2 pathPosition, Vector2 prevDest, Vector2 currentDest)
    {
        return (prevDest.y <= currentDest.y && pathPosition.y >= currentDest.y)
            || (prevDest.y >= currentDest.y && pathPosition.y <= currentDest.y)
            || (Mathf.Abs(pathPosition.y - currentDest.y) <= cBotMaxPositionError);
    }

    public void MoveTo(Vector2 destination)
    {
        MoveTo(new Vector2Int((int)destination.x, (int)destination.y));
    }

    public void MoveTo(Vector2Int destination)
    {
        Vector2Int startTile = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        var path = GameGrid.instance.mPathFinder.FindPath(
                    startTile,
                    destination,
                    Mathf.CeilToInt(mHeight),
                    Mathf.CeilToInt(mWidth),
                    (short)entity.maxJumpHeight);

        /*
        Debug.Log("Path");
        int count = 0;

        foreach (Vector2Int v in path)
        {
            count++;
            Debug.Log("node " + count + ": " + v);

        }
        */
        mPath.Clear();

        if (path != null && path.Count > 1)
        {
            for (var i = path.Count - 1; i >= 0; --i)
                mPath.Add(path[i]);

            mCurrentNodeId = 1;

            //ChangeAction(BotAction.MoveTo);

            mFramesOfJumping = GetJumpHeightForNode(0);
        }
        else
        {
            mCurrentNodeId = -1;

            states = SLIME_STATE.SLIMEATTACK1;
        }
    }

    public float GetJumpHeightForNode(int prevNodeId)
    {
        int currentNodeId = prevNodeId + 1;

        if (mPath[currentNodeId].y - mPath[prevNodeId].y > 0 && entity._controller.isGrounded)
        {
            int jumpHeight = 1;
            for (int i = currentNodeId; i < mPath.Count; ++i)
            {
                if (mPath[i].y - mPath[prevNodeId].y >= jumpHeight)
                    jumpHeight = mPath[i].y - mPath[prevNodeId].y;
                if (mPath[i].y - mPath[prevNodeId].y < jumpHeight || GameGrid.instance.IsGround(mPath[i].x, mPath[i].y-1))
                    return GetJumpFrameCount(jumpHeight);
            }
        }

        return 0;
    }

    float GetJumpFrameCount(int deltaY)
    {
        return Mathf.Min((float)deltaY, entity.maxJumpHeight);
    }

    private void OnDrawGizmos()
    {
        if(mPath == null || mPath.Count < 1)
        {
            return;
        }
        Gizmos.color = new Color(1.0f, 0.0f, 0.0f);

        foreach (Vector2Int point in mPath)
        {
            Gizmos.DrawWireSphere(new Vector3(point.x + 0.5f, point.y + 0.5f), 0.1f);
        }
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

