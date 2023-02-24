using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredPlatform : MovingPlatform
{

    public bool waiting = true;
    public EntityDirection direction = EntityDirection.Left;
    public PhysicsBody2D rider = null;
    public float waitCooldown = 1;
    public float waitTimeStamp = 0;
    //public GameObject itemTooltip;

    // Start is called before the first frame update
    void Start()
    {
        /*
        switch (movementType)
        {
            case MovingPlatformMovement.Horizontal:
                _velocity.x = MovementSpeed;
                break;
            case MovingPlatformMovement.Clockwise:
                _velocity.x = MovementSpeed;
                break;
            case MovingPlatformMovement.Vertical:
                _velocity.y = MovementSpeed;
                break;
            case MovingPlatformMovement.Counterclockwise:
                _velocity.x = -MovementSpeed;
                break;
        }
        */
    }

    protected override void Awake()
    {
        base.Awake();

    }



    // Update is called once per frame
    void Update()
    {
        //_controller.collisionState.
        if(waiting)
        {
            _controller.velocity = Vector2.zero;
            if(Time.time > waitTimeStamp + waitCooldown && _controller.riders.Count > 0)
            {
                waiting = false;

            }

        }
        else
        {
            TriggerMove();

        }


        _controller.move();

    }

    public void TriggerMove()
    {

        if((direction == EntityDirection.Right && _controller.collisionState.groundRight) || (direction == EntityDirection.Left && _controller.collisionState.groundLeft))
        {
            waiting = true;
            waitTimeStamp = Time.time;
            direction = (EntityDirection)((int)direction * -1);
        }
        else
        {
            _controller.velocity.x = (int)direction*MovementSpeed;

        }
       

    }

}
