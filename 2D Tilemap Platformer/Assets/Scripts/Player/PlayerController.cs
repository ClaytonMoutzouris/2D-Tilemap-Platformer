using UnityEngine;
using System.Collections;

public enum PlayerMovementState { Idle, Walking, Jumping, Falling, GrabLedge };

public class PlayerController : MonoBehaviour
{
	// movement config
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;

    public Vector3 ledgeGrabPosition;
    public Vector3 ledgeGrabOffset = new Vector3(0.3f, 0.6f, 0);

    public PlayerMovementState movementState = PlayerMovementState.Idle;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;

	private PhysicsBody2D _controller;
	public Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	public Vector3 _velocity;
    public AttackManager _attackManager;


	void Awake()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<PhysicsBody2D>();
        _attackManager = GetComponent<AttackManager>();

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
		Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
	}


	void onTriggerExitEvent( Collider2D col )
	{
        _controller.ignoreOneWayPlatformsThisFrame = false;
    }

    #endregion

    void AttemptLedgeGrab()
    {

        if(!_controller.isGrounded && !_attackManager.attacking && _controller.collisionState.canGrabLedge && _controller.velocity.y <= 0 && (_controller.collisionState.right || _controller.collisionState.left))
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


    // the Update loop contains a very simple example of moving the character around and controlling the animation
    void Update()
	{

        normalizedHorizontalSpeed = 0;


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
        }

        if (Input.GetKey(KeyCode.Space) && movementState != PlayerMovementState.GrabLedge)
        {
            int randomAttack = Random.Range(0, _attackManager.attacks.Count);
            _attackManager.ActivateAttack(randomAttack);

        }


        //Set animator state
        if (_attackManager.attacking)
        {
            _animator.Play(Animator.StringToHash("DEFAULT_ATTACK"));
            _animator.speed = _attackManager.activeAttack.attackData.attackSpeed;
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
            }
        }

        //I could probably move this stuff somewhere else

        // apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );

		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;

        if (movementState == PlayerMovementState.GrabLedge)
        {
            _velocity = Vector3.zero;
        }

        _controller.move( _velocity * Time.deltaTime );

		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
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

        // we can only jump whilst grounded or grabbing a ledge
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            movementState = PlayerMovementState.Jumping;


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


        // we can only jump whilst grounded or grabbing a ledge
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            movementState = PlayerMovementState.Jumping;


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
        if (_controller.isGrounded && !_controller.collisionState.wasGroundedLastFrame)
            movementState = PlayerMovementState.Idle;

        //_animator.Play(Animator.StringToHash("Jump"));


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
            _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            movementState = PlayerMovementState.Jumping;

        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            _velocity.y -= 1f;
            movementState = PlayerMovementState.Jumping;

        }
    }
}
