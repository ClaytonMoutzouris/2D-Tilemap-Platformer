using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    public EnemyEntity entity;
    public Entity target;


    public List<Vector2Int> mPath = new List<Vector2Int>();
    public int mCurrentNodeId = -1;
    Vector2 mDestination;
    [SerializeField]
    public float mFramesOfJumping = 0;
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
    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<EnemyEntity>();
        mWidth = entity._controller.boxCollider.size.x;
        mHeight = entity._controller.boxCollider.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToTarget(Entity target)
    {
        this.target = target;
        MoveTo(target.transform.position);
    }

    public void MoveToTarget()
    {
        MoveTo(target.transform.position);
    }

    public void JumperPathFollow()
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
            entity._controller.ignoreOneWayPlatformsThisFrame = true;
        }
        //mInputs[(int)KeyInput.GoDown] = true;



        if (reachedX && reachedY)
        {
            int prevNodeId = mCurrentNodeId;
            mCurrentNodeId++;
            //MoveTo(target.transform.position);
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

        if (entity._controller.isGrounded)
        {
            //Change this to just tell us if we need to jump or not
            float jumpHeight = GetJumpHeightForNextNode();

            if (jumpHeight > 0)
            {
                Jump(jumpHeight);

            }

        }

        if (Vector2.Distance(transform.position, entity.mOldPosition) == 0)
        {
            ++mStuckFrames;
            if (mStuckFrames > cMaxStuckFrames)
            {
                MoveTo(mPath[mPath.Count - 1]);

            }
        }
        else
        {
            mStuckFrames = 0;
        }
    }

    public void Jump(float jumpHeight)
    {
        if (!entity._controller.isGrounded)
        {
            //Can't jump while jumping or in the air
            return;
        }

        if (jumpHeight > entity.maxJumpHeight)
        {
            jumpHeight = entity.maxJumpHeight;
        }
        //Basically just set the velocity to the jump speed
        entity._velocity.y = Mathf.Sqrt(2 * (jumpHeight + 0.5f) * -GambleConstants.GRAVITY);

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

        if (reachedY)
        {
            //Debug.Log("Reach Y for " + currentDest);
        }

        if (reachedX)
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
        }
        else
        {
            mCurrentNodeId = -1;
        }
    }

    public float GetJumpHeightForNode(int prevNodeId)
    {
        //Calculate the trajectory here?
        int currentNodeId = prevNodeId + 1;

        if (mPath[currentNodeId].y - mPath[prevNodeId].y > 0 && entity._controller.isGrounded)
        {
            int jumpHeight = 1;
            for (int i = currentNodeId; i < mPath.Count; ++i)
            {
                if (mPath[i].y - mPath[prevNodeId].y >= jumpHeight)
                    jumpHeight = mPath[i].y - mPath[prevNodeId].y;
                if (mPath[i].y - mPath[prevNodeId].y < jumpHeight || GameGrid.instance.IsGround(mPath[i].x, mPath[i].y - 1))
                    return GetJumpFrameCount(jumpHeight);
            }
        }

        return 0;
    }

    public float GetJumpHeightForPreviousNode()
    {
        if(mCurrentNodeId == 0 || mPath.Count < 1)
        {
            return 0;
        }

        int prevNodeId = mCurrentNodeId - 1;

        if (mPath[mCurrentNodeId].y - mPath[prevNodeId].y > 0 && entity._controller.isGrounded)
        {
            int jumpHeight = 1;
            for (int i = mCurrentNodeId; i < mPath.Count; ++i)
            {
                if (mPath[i].y - mPath[prevNodeId].y >= jumpHeight)
                    jumpHeight = mPath[i].y - mPath[prevNodeId].y;
                if (mPath[i].y - mPath[prevNodeId].y < jumpHeight || GameGrid.instance.IsGround(mPath[i].x, mPath[i].y - 1))
                    return GetJumpFrameCount(jumpHeight);
            }
        }

        return 0;
    }

    public float GetJumpHeightForNextNode()
    {
        if(mPath.Count < 2 || mCurrentNodeId + 1 >= mPath.Count)
        {
            return 0;
        }

        int nextNodeID = mCurrentNodeId + 1;

        if (mPath[nextNodeID].y - mPath[mCurrentNodeId].y > 0 && entity._controller.isGrounded)
        {

            return GetJumpFrameCount(mPath[nextNodeID].y - mPath[mCurrentNodeId].y + 1);
            
        }

        return 0;
    }

    float GetJumpFrameCount(int deltaY)
    {
        return Mathf.Min((float)deltaY, entity.maxJumpHeight);
    }
    /*
    private void OnDrawGizmos()
    {
        if (mPath == null || mPath.Count < 1)
        {
            return;
        }
        Gizmos.color = new Color(1.0f, 0.0f, 0.0f);

        foreach (Vector2Int point in mPath)
        {
            Gizmos.DrawWireSphere(new Vector3(point.x + 0.5f, point.y + 0.5f), 0.1f);
        }
    }
    */
}
