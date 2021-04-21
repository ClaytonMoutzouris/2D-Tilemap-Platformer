﻿using UnityEngine;
using System.Collections;
using System;

public enum PlayerMovementState { Idle, Walking, Jumping, Falling, GrabLedge, Charge, Attacking, ClimbingLadder };

public class PlayerController : Entity
{
	// movement config
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;

    public Vector3 ladderClimbPosition;
    public Vector3 ledgeGrabPosition;
    public Vector3 ledgeGrabOffset = new Vector3(0.3f, 0.6f, 0);

    public bool IgnoreGravity = false;

    public PlayerMovementState movementState = PlayerMovementState.Idle;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;
    private float normalizedVerticalSpeed = 0;

    private PhysicsBody2D _controller;
    public Vector3 _velocity;
    public PlayerAttackManager _attackManager;

    protected override void Awake()
	{
        base.Awake();
        _controller = GetComponent<PhysicsBody2D>();
        _attackManager = GetComponent<PlayerAttackManager>();

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
            movementState = PlayerMovementState.GrabLedge;
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
            movementState = PlayerMovementState.ClimbingLadder;

            ladderClimbPosition = _controller.collisionState.ladderOffset;

            Debug.Log("Ladder Climb posititon " + ladderClimbPosition);
        }

    }


    // the Update loop contains a very simple example of moving the character around and controlling the animation
    void Update()
	{

        normalizedHorizontalSpeed = 0;
        normalizedVerticalSpeed = 0;
        IgnoreGravity = false;

        switch (movementState)
        {
            case PlayerMovementState.Idle:
                Idle();
                break;
            case PlayerMovementState.Walking:
                Walking();
                break;
            case PlayerMovementState.Jumping:
                Jumping();
                break;
            case PlayerMovementState.Falling:

                break;
            case PlayerMovementState.GrabLedge:
                GrabLedge();
                break;
            case PlayerMovementState.ClimbingLadder:
                Climbing();
                break;
            case PlayerMovementState.Charge:
                Charge();
                break;
            case PlayerMovementState.Attacking:

                break;
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            //int randomAttack = Random.Range(0, _attackManager.attacks.Count);

            _attackManager.SwapWeaponUp();

        }

        if (Input.GetKeyDown(KeyCode.Space) && movementState != PlayerMovementState.GrabLedge)
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
                case PlayerMovementState.Falling:

                    break;
                case PlayerMovementState.Idle:
                    _animator.Play(Animator.StringToHash("Idle"));

                    break;
                case PlayerMovementState.Jumping:
                    _animator.Play(Animator.StringToHash("Jump"));

                    break;
                case PlayerMovementState.Walking:
                    _animator.Play(Animator.StringToHash("Run"));
                    break;
                case PlayerMovementState.GrabLedge:
                    _animator.Play(Animator.StringToHash("Idle"));
                    break;
                case PlayerMovementState.ClimbingLadder:
                    _animator.Play(Animator.StringToHash("Idle"));
                    break;
            }
        }

        //I could probably move this stuff somewhere else

        // apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
        if(movementState != PlayerMovementState.Charge && movementState != PlayerMovementState.Attacking)
        {
            _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);
        }

        // apply gravity before moving
        if(!IgnoreGravity)
            _velocity.y += gravity * Time.deltaTime;

        if (movementState == PlayerMovementState.GrabLedge)
        {
            _velocity = Vector3.zero;
        }

        _controller.move( _velocity * Time.deltaTime );

		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
	}

    private void Climbing()
    {
        _controller.transform.position = new Vector2(ladderClimbPosition.x, _controller.transform.position.y);
        IgnoreGravity = true;
        
        //_velocity = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            normalizedVerticalSpeed = 1;

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            normalizedVerticalSpeed = -1;
        }

        _velocity.y = Mathf.Lerp(_velocity.y, normalizedVerticalSpeed * runSpeed, Time.deltaTime * groundDamping);

        if (Input.GetKeyDown(KeyCode.J) && Input.GetKey(KeyCode.DownArrow))
        {
            movementState = PlayerMovementState.Jumping;

            return;
        } else if (Input.GetKeyDown(KeyCode.J))
        {
            Jump();
            return;
        }



        if (!_controller.collisionState.onLadder)
        {
            movementState = PlayerMovementState.Idle;
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
            movementState = PlayerMovementState.Jumping;
            return;
        }

        _velocity.y = 0;


        if (Input.GetKey(KeyCode.RightArrow))
        {
            normalizedHorizontalSpeed = 1;
            if (transform.localScale.x < 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            movementState = PlayerMovementState.Walking;

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            normalizedHorizontalSpeed = -1;
            if (transform.localScale.x > 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            movementState = PlayerMovementState.Walking;

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
            movementState = PlayerMovementState.Jumping;
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

            movementState = PlayerMovementState.Idle;

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
            movementState = PlayerMovementState.Idle;

        //_animator.Play(Animator.StringToHash("Jump"));

        if (!Input.GetKey(KeyCode.UpArrow) && _velocity.y > 0.0f)
        {
            _velocity.y = Mathf.Min(_velocity.y, Mathf.Sqrt(jumpHeight * -gravity));
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
            movementState = PlayerMovementState.Jumping;

        }
    }

    public void Jump()
    {
        if(movementState == PlayerMovementState.Jumping)
        {
            //Can't jump while jumping or in the air
            return;
        }

        _velocity.y = Mathf.Sqrt(2*jumpHeight * -gravity);
        movementState = PlayerMovementState.Jumping;

    }
    
    public int GetDirection()
    {
        return 1 * (int)Mathf.Sign(transform.localScale.x);
    }
}
