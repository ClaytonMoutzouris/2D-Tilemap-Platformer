using UnityEngine;
using System.Collections;
using System;

public enum MovementState { Idle, Walking, Jumping, Falling, GrabLedge, Charge, Attacking, ClimbingLadder };

public class PlayerController : Entity
{
	// movement config
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;

    public Vector3 ladderClimbPosition;
    public Vector3 ledgeGrabPosition;
    public Vector3 ledgeGrabOffset = new Vector3(0.3f, 0.6f, 0);

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;
    private float normalizedVerticalSpeed = 0;

    private PhysicsBody2D _controller;

    protected override void Awake()
	{
        base.Awake();
        _controller = GetComponent<PhysicsBody2D>();
        _attackManager = GetComponent<AttackManager>();
        _animator.runtimeAnimatorController = overrideController;

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
	}


	#region Event Listeners

	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}


	void onTriggerEnterEvent( Collider2D col )
	{
		//Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );

	}


	void onTriggerExitEvent( Collider2D col )
	{
        _controller.ignoreOneWayPlatformsThisFrame = false;
    }

    #endregion

    void AttemptLedgeGrab()
    {

        if(!_controller.isGrounded && !_attackManager.IsAttacking() && _controller.collisionState.canGrabLedge && _controller.velocity.y <= 0 && (_controller.collisionState.right || _controller.collisionState.left))
        {
            movementState = MovementState.GrabLedge;
            if(_controller.collisionState.right)
            {
                ledgeGrabOffset.x = -0.3f;
            } else if (_controller.collisionState.left)
            {
                ledgeGrabOffset.x = 0.3f;
            }
            ledgeGrabPosition = _controller.collisionState.ledgeGrabPosition + ledgeGrabOffset;
            Debug.Log(ledgeGrabPosition);
        }

    }

    void AttemptClimbLadder()
    {
        Debug.Log("Attempt Ladder");

        if (!_attackManager.IsAttacking())
        {
            movementState = MovementState.ClimbingLadder;

            ladderClimbPosition = _controller.collisionState.ladderOffset;

            Debug.Log("Ladder Climb posititon " + ladderClimbPosition);
        }

    }


    // the Update loop contains a very simple example of moving the character around and controlling the animation
    void Update()
	{

        normalizedHorizontalSpeed = 0;
        normalizedVerticalSpeed = 0;

        ignoreGravity = false;


        switch (movementState)
        {
            case MovementState.Idle:
                Idle();
                break;
            case MovementState.Walking:
                Walking();
                break;
            case MovementState.Jumping:
                Jumping();
                break;
            case MovementState.Falling:

                break;
            case MovementState.GrabLedge:
                GrabLedge();
                break;
            case MovementState.ClimbingLadder:
                Climbing();
                break;
            case MovementState.Charge:
                Charge();
                break;
            case MovementState.Attacking:

                break;
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            //int randomAttack = Random.Range(0, _attackManager.attacks.Count);

            _attackManager.SwapWeaponUp();

        }

        if (Input.GetKeyDown(KeyCode.Space) && movementState != MovementState.GrabLedge)
        {
            //int randomAttack = Random.Range(0, _attackManager.attacks.Count);
            
            _attackManager.ActivateWeaponAttack();

        }


        //Set animator state
        if (_attackManager.IsAttacking())
        {
            //attacking overrides other things
        }
        else
        {
            _animator.speed = 1;

            switch (movementState)
            {
                case MovementState.Falling:

                    break;
                case MovementState.Idle:
                    _animator.Play(Animator.StringToHash("Idle"));

                    break;
                case MovementState.Jumping:
                    _animator.Play(Animator.StringToHash("Jump"));

                    break;
                case MovementState.Walking:
                    _animator.Play(Animator.StringToHash("Run"));
                    break;
                case MovementState.GrabLedge:
                    _animator.Play(Animator.StringToHash("Idle"));
                    break;
                case MovementState.ClimbingLadder:
                    _animator.Play(Animator.StringToHash("Idle"));
                    break;
            }
        }

        //I could probably move this stuff somewhere else

        // apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
        if(movementState != MovementState.Charge && movementState != MovementState.Attacking)
        {
            _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * movementSpeed, Time.deltaTime * smoothedMovementFactor);
        }

        if (movementState == MovementState.GrabLedge)
        {
            _velocity = Vector3.zero;
        }

        if (!ignoreGravity)
            _velocity.y += GambleConstants.GRAVITY * Time.deltaTime;

        _controller.move( _velocity * Time.deltaTime );

		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
	}

    private void Climbing()
    {
        _controller.transform.position = new Vector2(ladderClimbPosition.x, _controller.transform.position.y);
        ignoreGravity = true;
        
        //_velocity = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            normalizedVerticalSpeed = 1;

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            normalizedVerticalSpeed = -1;
        }

        _velocity.y = Mathf.Lerp(_velocity.y, normalizedVerticalSpeed * movementSpeed, Time.deltaTime * groundDamping);

        if (Input.GetKeyDown(KeyCode.J) && Input.GetKey(KeyCode.DownArrow))
        {
            movementState = MovementState.Jumping;

            return;
        } else if (Input.GetKeyDown(KeyCode.J))
        {
            Jump();
            return;
        }



        if (!_controller.collisionState.onLadder)
        {
            movementState = MovementState.Idle;
        }



    }

    private void Charge()
    {
        //normalizedHorizontalSpeed = GetDirection()*2;
        //_controller.
    }

    //process the character movement when Idle
    void Idle()
    {

        if (!_controller.isGrounded)
        {
            movementState = MovementState.Jumping;
            return;
        }

        _velocity.y = 0;


        if (Input.GetKey(KeyCode.RightArrow))
        {
            normalizedHorizontalSpeed = 1;
            if (transform.localScale.x < 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            movementState = MovementState.Walking;

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            normalizedHorizontalSpeed = -1;
            if (transform.localScale.x > 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            movementState = MovementState.Walking;

        }

        if (Input.GetKey(KeyCode.UpArrow) && _controller.collisionState.onLadder)
        {
            AttemptClimbLadder();
            //return;
        }

        // we can only jump whilst grounded or grabbing a ledge
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

                Jump();

        }

        // if holding down bump up our movement amount and turn off one way platform detection for a frame.
        // this lets us jump down through one way platforms
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _velocity.y -= 2f;
            _controller.ignoreOneWayPlatformsThisFrame = true;

            //movementState = PlayerMovementState.Jumping;

        }
    }

    //process the character movement when walking
    void Walking()
    {
        if (!_controller.isGrounded)
        {
            movementState = MovementState.Jumping;
            return;
        }



        _velocity.y = 0;



        if (Input.GetKey(KeyCode.RightArrow))
        {
            normalizedHorizontalSpeed = 1;
            if (transform.localScale.x < 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            normalizedHorizontalSpeed = -1;
            if (transform.localScale.x > 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }
        else
        {
            normalizedHorizontalSpeed = 0;

            movementState = MovementState.Idle;

        }

        if (Input.GetKey(KeyCode.UpArrow) && _controller.collisionState.onLadder)
        {
            AttemptClimbLadder();
            //return;
        }

        // we can only jump whilst grounded or grabbing a ledge
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
                Jump();
        }

        // if holding down bump up our movement amount and turn off one way platform detection for a frame.
        // this lets us jump down through one way platforms
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _velocity.y -= 1f;
            _controller.ignoreOneWayPlatformsThisFrame = true;
            //movementState = PlayerMovementState.Jumping;

        }
    }

    void Jumping()
    {
        if (_controller.isGrounded && (!_controller.collisionState.wasGroundedLastFrame || _velocity.y <= 0))
            movementState = MovementState.Idle;

        //_animator.Play(Animator.StringToHash("Jump"));

        if (!Input.GetKey(KeyCode.UpArrow) && _velocity.y > 0.0f)
        {
            _velocity.y = Mathf.Min(_velocity.y, Mathf.Sqrt(jumpHeight*-GambleConstants.GRAVITY));
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            normalizedHorizontalSpeed = 1;
            if (transform.localScale.x < 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            normalizedHorizontalSpeed = -1;
            if (transform.localScale.x > 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }

        AttemptLedgeGrab();

        if(Input.GetKey(KeyCode.UpArrow) && _controller.collisionState.onLadder)
        {
            AttemptClimbLadder();
        }
        /*
        // we can only jump whilst grounded or grabbing a ledge
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);

        }
        */
    }

    void GrabLedge()
    {
        _velocity = Vector3.zero;
        _controller.transform.position = ledgeGrabPosition;
        /*
        if (Input.GetKey(KeyCode.RightArrow) && !_controller.collisionState.right)
        {
            normalizedHorizontalSpeed = 1;
            if (transform.localScale.x < 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            movementState = PlayerMovementState.Jumping;

        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !_controller.collisionState.left)
        {
            normalizedHorizontalSpeed = -1;
            if (transform.localScale.x > 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            movementState = PlayerMovementState.Jumping;

        }
        */

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();

        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            _velocity.y -= 1f;
            movementState = MovementState.Jumping;

        }
    }

    public void Jump()
    {
        if(movementState == MovementState.Jumping)
        {
            //Can't jump while jumping or in the air
            return;
        }


        _velocity.y = Mathf.Sqrt(2*jumpHeight*-GambleConstants.GRAVITY);
        movementState = MovementState.Jumping;

    }
    

}
