using UnityEngine;
using System.Collections;
using System;

public enum PlayerMovementState { Idle, Run, Jump, Falling, GrabLedge, Attacking, ClimbingLadder, Knockedback, Dead };

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

    public PhysicsBody2D _controller;
    public PlayerInputController _input;

    //Why not just use the controllers velocity?
    public Vector3 _velocity;

    public float knockbackTimestamp;
    public float knockbackDuration = 0;

    public PlayerMovementState movementState = PlayerMovementState.Idle;


    protected override void Awake()
	{
        base.Awake();
        _controller = GetComponent<PhysicsBody2D>();
        _attackManager = GetComponent<AttackManager>();
                _input = GetComponent<PlayerInputController>();

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

        ignoreGravity = false;


        switch (movementState)
        {
            case PlayerMovementState.Idle:
                Idle();
                break;
            case PlayerMovementState.Run:
                Walking();
                break;
            case PlayerMovementState.Jump:
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
            case PlayerMovementState.Attacking:

                break;
            case PlayerMovementState.Knockedback:
                Knockedback();
                break;
            case PlayerMovementState.Dead:
                if(_input.GetButtonDown(ButtonInput.Jump))
                {
                    StartCoroutine(Ressurect());
                }
                break;
        }

        if (_input.GetButtonDown(ButtonInput.Interact)) {
            //int randomAttack = Random.Range(0, _attackManager.attacks.Count);

            _attackManager.SwapWeaponUp();

        }

        if (_input.GetButtonDown(ButtonInput.MeleeAttack) && movementState != PlayerMovementState.GrabLedge && movementState != PlayerMovementState.Knockedback)
        {
            //int randomAttack = Random.Range(0, _attackManager.attacks.Count);
            
            _attackManager.ActivateAttack();

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
                case PlayerMovementState.Jump:
                    _animator.Play(Animator.StringToHash("Jump"));

                    break;
                case PlayerMovementState.Run:
                    _animator.Play(Animator.StringToHash("Run"));
                    break;
                case PlayerMovementState.GrabLedge:
                    _animator.Play(Animator.StringToHash("Idle"));
                    break;
                case PlayerMovementState.ClimbingLadder:
                    _animator.Play(Animator.StringToHash("Idle"));
                    break;
                case PlayerMovementState.Dead:
                    _animator.Play(Animator.StringToHash("Dead"));
                    break;
                    //case PlayerMovementState.Knockedback:

            }
        }

        //I could probably move this stuff somewhere else

        // apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
        if(movementState != PlayerMovementState.Attacking && movementState != PlayerMovementState.Knockedback
            && (movementState != PlayerMovementState.Dead || (normalizedHorizontalSpeed == 0 && _controller.isGrounded)))
        {
            _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * movementSpeed, Time.deltaTime * smoothedMovementFactor);
        }

        if (movementState == PlayerMovementState.GrabLedge)
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

        if (_input.GetAxisValue(AxisInput.LeftStickY) > 0)
        {
            normalizedVerticalSpeed = 1;

        }
        else if (_input.GetAxisValue(AxisInput.LeftStickY) < 0)
        {
            normalizedVerticalSpeed = -1;
        }

        _velocity.y = Mathf.Lerp(_velocity.y, normalizedVerticalSpeed * movementSpeed, Time.deltaTime * groundDamping);

        if (_input.GetButtonDown(ButtonInput.Jump) && _input.GetAxisValue(AxisInput.LeftStickY) < 0)
        {
            movementState = PlayerMovementState.Jump;

            return;
        } else if (_input.GetButtonDown(ButtonInput.Jump))
        {
            Jump();
            return;
        }



        if (!_controller.collisionState.onLadder)
        {
            movementState = PlayerMovementState.Idle;
        }



    }

    //process the character movement when Idle
    void Idle()
    {
        if (!_controller.isGrounded)
        {
            movementState = PlayerMovementState.Jump;
            return;
        }

        _velocity.y = 0;


        if (_input.GetAxisValue(AxisInput.LeftStickX) > 0.5f)
        {
            normalizedHorizontalSpeed = 1;
            if (transform.localScale.x < 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            movementState = PlayerMovementState.Run;

        }
        else if (_input.GetAxisValue(AxisInput.LeftStickX) < -0.5f)
        {
            normalizedHorizontalSpeed = -1;
            if (transform.localScale.x > 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            movementState = PlayerMovementState.Run;

        }

        if (_input.GetButton(ButtonInput.Jump) && _controller.collisionState.onLadder)
        { 
            AttemptClimbLadder();
            //return;
        }

        // we can only jump whilst grounded or grabbing a ledge
        if (_input.GetButtonDown(ButtonInput.Jump))
        {

                Jump();

        }

        // if holding down bump up our movement amount and turn off one way platform detection for a frame.
        // this lets us jump down through one way platforms
        if (_input.GetLeftStickTapDown())
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
            movementState = PlayerMovementState.Jump;
            return;
        }



        _velocity.y = 0;



        if (_input.GetAxisValue(AxisInput.LeftStickX) > 0.5f)
        {
            normalizedHorizontalSpeed = 1;
            if (transform.localScale.x < 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }
        else if (_input.GetAxisValue(AxisInput.LeftStickX) < -0.5f)
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

        if (_input.GetAxisValue(AxisInput.LeftStickY) != 0 && _controller.collisionState.onLadder)
        {
            AttemptClimbLadder();
            //return;
        }

        // we can only jump whilst grounded or grabbing a ledge
        if (_input.GetButtonDown(ButtonInput.Jump))
        {
                Jump();
        }

        // if holding down bump up our movement amount and turn off one way platform detection for a frame.
        // this lets us jump down through one way platforms
        if (_input.GetLeftStickTapDown())
        {
            _velocity.y -= 1f;
            _controller.ignoreOneWayPlatformsThisFrame = true;
            //movementState = PlayerMovementState.Jumping;

        }
    }

    void Jumping()
    {
        if (_controller.isGrounded && (!_controller.collisionState.wasGroundedLastFrame || _velocity.y <= 0))
        {
            movementState = PlayerMovementState.Idle;
            return;
        }

        //_animator.Play(Animator.StringToHash("Jump"));

        if (!_input.GetButton(ButtonInput.Jump) && _velocity.y > 0.0f)
        {
            _velocity.y = Mathf.Min(_velocity.y, Mathf.Sqrt(jumpHeight*-GambleConstants.GRAVITY));
        }

        if (_input.GetAxisValue(AxisInput.LeftStickX) > 0.5f)
        {
            normalizedHorizontalSpeed = 1;
            if (transform.localScale.x < 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }
        else if (_input.GetAxisValue(AxisInput.LeftStickX) < -0.5f)
        {
            normalizedHorizontalSpeed = -1;
            if (transform.localScale.x > 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }

        AttemptLedgeGrab();

        if(_input.GetAxisValue(AxisInput.LeftStickY) > 0.5f && _controller.collisionState.onLadder)
        {
            AttemptClimbLadder();
        }
        /*
        // we can only jump whilst grounded or grabbing a ledge
        if (_input.(KeyCode.UpArrow))
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
        if (_input.(KeyCode.RightArrow) && !_controller.collisionState.right)
        {
            normalizedHorizontalSpeed = 1;
            if (transform.localScale.x < 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            movementState = PlayerMovementState.Jumping;

        }
        else if (_input.(KeyCode.LeftArrow) && !_controller.collisionState.left)
        {
            normalizedHorizontalSpeed = -1;
            if (transform.localScale.x > 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            movementState = PlayerMovementState.Jumping;

        }
        */

        if (_input.GetButtonDown(ButtonInput.Jump))
        {
            Jump();

        }

        if (_input.GetLeftStickTapDown())
        {
            _velocity.y -= 1f;
            movementState = PlayerMovementState.Jump;

        }
    }

    public void Jump()
    {
        if(movementState == PlayerMovementState.Jump || _controller.collisionState.becameGroundedThisFrame)
        {
            //Can't jump while jumping or in the air
            return;
        }


        _velocity.y = Mathf.Sqrt(2*jumpHeight*-GambleConstants.GRAVITY);
        movementState = PlayerMovementState.Jump;

    }

    public override void GetHit(AttackObject attack)
    {
        //Debug.Log(name + " was hit by attack " + attack + " for " + attack.attackData.Damage);
        base.GetHit(attack);

        Projectile proj = attack.GetComponent<Projectile>();
        Vector2 difference;

        if (proj) {
            difference = (transform.position - attack.transform.position).normalized;
        }
        else
        {
            difference = (transform.position - attack.owner.transform.position).normalized;
        }


        _velocity = difference * 5 + Vector2.up*Mathf.Sqrt(-GambleConstants.GRAVITY);


        movementState = PlayerMovementState.Knockedback;

        knockbackTimestamp = Time.time;
        knockbackDuration = 0.3f;

        if (health.currentHealth <= 0)
        {
            Debug.Log("Player died");
            movementState = PlayerMovementState.Dead;
        }
    }

    private void Knockedback()
    {
        if(Time.time >= knockbackTimestamp + knockbackDuration)
        {
            movementState = PlayerMovementState.Idle;
            knockbackDuration = 0;
        }
    }

    public IEnumerator Ressurect()
    {
        float resTimestamp = Time.time;
        int resCounter = 0;
        int resCountFull = 4;

        while(movementState == PlayerMovementState.Dead)
        {
            if (!_input.GetButton(ButtonInput.Jump)) {
                yield break;
            }

            if (Time.time >= resTimestamp + (resCounter + 1))
            {
                resCounter++;
                if (resCounter >= resCountFull)
                {
                    movementState = PlayerMovementState.Idle;
                    health.SetHealth(20);
                    Debug.Log("Player revived");
                    break;
                }

                ShowFloatingText(resCounter + "", Color.green);
            }



            yield return null;
        }

    }

}
