using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum PlayerMovementState { Idle, Run, Jump, Falling, GrabLedge, Attacking, ClimbingLadder, Dead, Roll, Crouch };

public class PlayerController : Entity
{
	// movement config
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;

    public PlayerVersusUI playerVersusUI;

    public Vector3 ladderClimbPosition;
    public Vector3 ledgeGrabPosition;
    public Vector3 ledgeGrabOffset = new Vector3(0.2f, 0.6f, 0);

	public float normalizedHorizontalSpeed = 0;
    public float normalizedVerticalSpeed = 0;

    public PhysicsBody2D _controller;
    public PlayerInputController _input;
    public EquipmentManager _equipmentManager;
    public TalentTree talentTree;
    public List<Talent> learnedTalents = new List<Talent>();

    public float rollSpeed = 2;
    public LayerMask rollMask;

    //Why not just use the controllers velocity?

    public float knockbackTimestamp;
    public float knockbackDuration = 0;

    float climbTimeStamp;
    float climbCooldown = 0.3f;

    public int playerIndex = 0;

    public PlayerMovementState movementState = PlayerMovementState.Idle;
    public Light playerLight;

    //Combine these
    public ColorSwap colorSwap;
    public ColorSwapper colorSwapper;
    public ParticleSystem deathEffect;

    public ContactFilter2D itemFilter;
    public PlayerCreationData playerData;


    protected override void Awake()
	{
        base.Awake();
        _controller = GetComponent<PhysicsBody2D>();
        _attackManager = GetComponent<AttackManager>();
        _input = GetComponent<PlayerInputController>();
        _equipmentManager = GetComponent<EquipmentManager>();

        _animator.runtimeAnimatorController = overrideController;

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
        colorSwap = new ColorSwap(spriteRenderer.material);
        colorSwapper = new ColorSwapper(spriteRenderer.material);

    }

    public void SetData(PlayerCreationData data)
    {
        playerData = data;

        colorSwap.SetBaseColors(playerData.playerColors);

        foreach (Talent talent in playerData.talents)
        {
            Talent newTalent = Instantiate(talent);
            newTalent.LearnTalent(this);
        }
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
        if(col.gameObject.layer == LayerMask.NameToLayer("OneWayPlatform"))
        {
            _controller.ignoreOneWayPlatformsThisFrame = false;
        }

    }

    #endregion

    void AttemptLedgeGrab()
    {

        if(!_controller.isGrounded && movementState != PlayerMovementState.Attacking && _controller.collisionState.canGrabLedge && _controller.velocity.y <= 0 && (_controller.collisionState.right || _controller.collisionState.left))
        {
            movementState = PlayerMovementState.GrabLedge;
            if(_controller.collisionState.right)
            {
                ledgeGrabOffset.x = -Mathf.Abs(ledgeGrabOffset.x);
            } else if (_controller.collisionState.left)
            {
                ledgeGrabOffset.x = Mathf.Abs(ledgeGrabOffset.x);
            }
            ledgeGrabPosition = _controller.collisionState.ledgeGrabPosition + ledgeGrabOffset;
            Debug.Log(ledgeGrabPosition);
        }

    }

    bool AttemptClimbLadder()
    {
        if(Time.time < climbTimeStamp + climbCooldown)
        {
            return false;
        }

        if (movementState != PlayerMovementState.Attacking)
        {
            movementState = PlayerMovementState.ClimbingLadder;

            ladderClimbPosition = _controller.collisionState.ladderOffset;
            _controller.ignoreOneWayPlatformsThisFrame = true;

            return true;
        }

        return false;
    }


    // the Update loop contains a very simple example of moving the character around and controlling the animation
    void Update()
	{

        if (movementState != PlayerMovementState.Roll)
        {
            normalizedHorizontalSpeed = 0;
        }
        normalizedVerticalSpeed = 0;

        ignoreGravity = false;


        switch (movementState)
        {
            case PlayerMovementState.Idle:
                Idle();
                break;
            case PlayerMovementState.Crouch:
                Crouch();
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
                Attacking();
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

            //_attackManager.SwapWeaponUp();

        }

        if (_input.GetButtonDown(ButtonInput.LightAttack) && movementState != PlayerMovementState.GrabLedge
            && movementState != PlayerMovementState.Roll && movementState != PlayerMovementState.Dead && !knockedBack)
        {
            //int randomAttack = Random.Range(0, _attackManager.attacks.Count);
            
            _attackManager.ActivateAttack(ButtonInput.LightAttack);

        }

        if (_input.GetButtonDown(ButtonInput.Fire) && movementState != PlayerMovementState.GrabLedge
            && movementState != PlayerMovementState.Roll && movementState != PlayerMovementState.Dead && !knockedBack)
        {
            //int randomAttack = Random.Range(0, _attackManager.attacks.Count);

            _attackManager.ActivateAttack(ButtonInput.Fire);

        }

        if (_input.GetButtonDown(ButtonInput.HeavyAttack) && movementState != PlayerMovementState.GrabLedge
            && movementState != PlayerMovementState.Roll && movementState != PlayerMovementState.Dead && !knockedBack)
        {
            //int randomAttack = Random.Range(0, _attackManager.attacks.Count);

            _attackManager.ActivateHeavyAttack(ButtonInput.HeavyAttack);

        }

        if (_input.GetButtonDown(ButtonInput.Roll) && movementState != PlayerMovementState.GrabLedge
            && movementState != PlayerMovementState.Attacking && !knockedBack
            && movementState != PlayerMovementState.Roll && movementState != PlayerMovementState.Dead)
        {
            StartCoroutine(Rolling());
        }

        //See if we are colliding with any items
        List<ItemObject> itemsTouching = CheckForItems();


        if (itemsTouching.Count > 0)
        {
            //itemsTouching[0].ShowTooltip(this);
            if(playerVersusUI != null && playerVersusUI.showItemTooltips)
            {
                playerVersusUI.tooltip.ShowTooltip(itemsTouching[0]);
            }


            if (_input.GetLeftStickTapDown())
            {
                PickupItem(itemsTouching[0]);
            }
        } else
        {
            if (playerVersusUI != null)
            {
                playerVersusUI.tooltip.HideTooltip();
            }
        }



        switch (movementState)
        {
            case PlayerMovementState.Falling:
                _animator.speed = 1;

                break;
            case PlayerMovementState.Idle:
                _animator.speed = 1;

                _animator.Play(Animator.StringToHash("Idle"));

                break;
            case PlayerMovementState.Crouch:
                _animator.speed = 1;

                _animator.Play(Animator.StringToHash("Crouch"));

                break;
            case PlayerMovementState.Jump:
                _animator.speed = 1;

                _animator.Play(Animator.StringToHash("Jump"));

                break;
            case PlayerMovementState.Run:
                _animator.speed = 1;

                _animator.Play(Animator.StringToHash("Run"));
                break;
            case PlayerMovementState.GrabLedge:
                _animator.speed = 1;

                _animator.Play(Animator.StringToHash("GrabLedge"));
                break;
            case PlayerMovementState.ClimbingLadder:
                _animator.speed = 1;

                if (normalizedVerticalSpeed != 0)
                {
                    _animator.Play(Animator.StringToHash("ClimbingLadder"));
                } else
                {
                    _animator.Play(Animator.StringToHash("ClimbingLadderIdle"));
                }
                break;
            case PlayerMovementState.Dead:
                _animator.speed = 1;
                _animator.Play(Animator.StringToHash("Dead"));
                break;
            case PlayerMovementState.Attacking:
                //attacks should handle their own animations

                break;
            //case PlayerMovementState.Knockedback:
        }
        

        //Debug.Log("Movement State This Frame " + movementState);
        //I could probably move this stuff somewhere else

        // apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
        if(!knockedBack && (movementState != PlayerMovementState.Dead || (normalizedHorizontalSpeed == 0 && _controller.isGrounded)))
        {
            _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * stats.GetSecondaryStat(SecondaryStatType.MoveSpeed).GetValue(), Time.deltaTime * smoothedMovementFactor);
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

        if(movementState != PlayerMovementState.Dead && _controller.collisionState.spiked)
        {
            Debug.Log(name + " has been spiked");
            health.LoseHealth(999);
        }
	}

    private void Attacking()
    {

        normalizedHorizontalSpeed = 0;
        

        if (_input.GetAxisValue(AxisInput.LeftStickX) > 0.5f)
        {
            normalizedHorizontalSpeed = 1;
            //SetDirection(EntityDirection.Right);


        }
        else if (_input.GetAxisValue(AxisInput.LeftStickX) < -0.5f)
        {
            normalizedHorizontalSpeed = -1;
            //SetDirection(EntityDirection.Left);
        }

        if(_controller.isGrounded)
        {
            normalizedHorizontalSpeed *= .35f;
        }
    }

    private void Climbing()
    {
        climbTimeStamp = Time.time;

        _controller.transform.position = new Vector2(ladderClimbPosition.x, _controller.transform.position.y);
        ignoreGravity = true;
        
        //_velocity = Vector3.zero;

        if (_input.GetAxisValue(AxisInput.LeftStickY) > 0)
        {
            normalizedVerticalSpeed = 1;

        }
        else if (_input.GetAxisValue(AxisInput.LeftStickY) < 0)
        {
            if(_controller.isGrounded)
            {
                movementState = PlayerMovementState.Idle;
                return;
            }

            if (_input.GetButtonDown(ButtonInput.Jump))
            {
                movementState = PlayerMovementState.Jump;
                return;
            }

            normalizedVerticalSpeed = -1;

        }

        _velocity.y = Mathf.Lerp(_velocity.y, normalizedVerticalSpeed * stats.GetSecondaryStat(SecondaryStatType.MoveSpeed).GetValue(), Time.deltaTime * groundDamping);

        if (_input.GetButtonDown(ButtonInput.Jump))
        {
            Jump();
            return;
        }

        /*
        if(_input.GetLeftStickTapLeft() || _input.GetLeftStickTapRight())
        {
            movementState = PlayerMovementState.Jump;
        }
        */


        if (!_controller.collisionState.onLadder)
        {
            movementState = PlayerMovementState.Idle;
        }



    }

    //process the character movement when Idle
    void Idle()
    {

        normalizedHorizontalSpeed = 0;

        if (!_controller.isGrounded || knockedBack)
        {
            movementState = PlayerMovementState.Jump;
            return;
        }

        _velocity.y = 0;


        if (_input.GetAxisValue(AxisInput.LeftStickX) > 0.5f)
        {
            normalizedHorizontalSpeed = 1;
            SetDirection(EntityDirection.Right);

            movementState = PlayerMovementState.Run;

        }
        else if (_input.GetAxisValue(AxisInput.LeftStickX) < -0.5f)
        {
            normalizedHorizontalSpeed = -1;
            SetDirection(EntityDirection.Left);


            movementState = PlayerMovementState.Run;

        }

        if (_input.GetAxisValue(AxisInput.LeftStickY) > 0.5f && _controller.collisionState.onLadder)
        {
            if (AttemptClimbLadder())
            {
                return;
            }
            //return;
        }

        // we can only jump whilst grounded or grabbing a ledge
        if (_input.GetButtonDown(ButtonInput.Jump))
        {

                Jump();

        }

        // if holding down bump up our movement amount and turn off one way platform detection for a frame.
        // this lets us jump down through one way platforms
        if (_input.GetLeftStickTapDown() && _controller.collisionState.onOneWayPlatform)
        {

            _velocity.y -= 1f;
            _controller.ignoreOneWayPlatformsThisFrame = true;

            //movementState = PlayerMovementState.Jumping;

        }

        if (_input.GetLeftStickHoldDown())
        {
            movementState = PlayerMovementState.Crouch;
            return;
        }
    }

    //process the character movement when Idle
    void Crouch()
    {

        normalizedHorizontalSpeed = 0;

        if (!_controller.isGrounded || knockedBack)
        {
            movementState = PlayerMovementState.Jump;
            return;
        }

        if(_input.GetAxisValue(AxisInput.LeftStickY) >= 0)
        {
            movementState = PlayerMovementState.Idle;
            return;
        }

        _velocity.y = 0;


        if (_input.GetAxisValue(AxisInput.LeftStickX) > 0.5f)
        {
            normalizedHorizontalSpeed = 1;
            SetDirection(EntityDirection.Right);


            movementState = PlayerMovementState.Run;

        }
        else if (_input.GetAxisValue(AxisInput.LeftStickX) < -0.5f)
        {
            normalizedHorizontalSpeed = -1;
            SetDirection(EntityDirection.Left);


            movementState = PlayerMovementState.Run;

        }

        if (_input.GetAxisValue(AxisInput.LeftStickY) > 0.5f && _controller.collisionState.onLadder)
        {
            if (AttemptClimbLadder())
            {
                return;
            }
        }

        // we can only jump whilst grounded or grabbing a ledge
        if (_input.GetButtonDown(ButtonInput.Jump))
        {

            Jump();

        }

        // if holding down bump up our movement amount and turn off one way platform detection for a frame.
        // this lets us jump down through one way platforms
        if (_input.GetLeftStickTapDown() && _controller.collisionState.onOneWayPlatform)
        {
            _velocity.y -= 2f;
            _controller.ignoreOneWayPlatformsThisFrame = true;

            //movementState = PlayerMovementState.Jumping;

        }
    }

    //process the character movement when walking
    void Walking()
    {
        normalizedHorizontalSpeed = 0;

        if (!_controller.isGrounded)
        {
            movementState = PlayerMovementState.Jump;
            return;
        }



        _velocity.y = 0;

        foreach (Ability ability in abilities)
        {
            if(ability is EffectOnWalk onWalk)
            {
                onWalk.OnWalk();
            }
        }

        if (_input.GetAxisValue(AxisInput.LeftStickX) > 0.5f)
        {
            normalizedHorizontalSpeed = 1;
            SetDirection(EntityDirection.Right);


        }
        else if (_input.GetAxisValue(AxisInput.LeftStickX) < -0.5f)
        {
            normalizedHorizontalSpeed = -1;
            SetDirection(EntityDirection.Left);


        }
        else
        {
            normalizedHorizontalSpeed = 0;

            movementState = PlayerMovementState.Idle;

        }

        if (_input.GetAxisValue(AxisInput.LeftStickY) > 0.5f && _controller.collisionState.onLadder)
        {
            if (AttemptClimbLadder())
            {
                return;
            }
        }

        // we can only jump whilst grounded or grabbing a ledge
        if (_input.GetButtonDown(ButtonInput.Jump))
        {
                Jump();
        }

        // if holding down bump up our movement amount and turn off one way platform detection for a frame.
        // this lets us jump down through one way platforms
        if (_input.GetLeftStickTapDown() && _controller.collisionState.onOneWayPlatform)
        {
            _velocity.y -= 1f;
            _controller.ignoreOneWayPlatformsThisFrame = true;
            //movementState = PlayerMovementState.Jumping;

        }
    }

    void Jumping()
    {
        normalizedHorizontalSpeed = 0;

        if (_controller.isGrounded /*&& (!_controller.collisionState.wasGroundedLastFrame || _velocity.y <= 0)*/)
        {
            movementState = PlayerMovementState.Idle;
            return;
        }

        //_animator.Play(Animator.StringToHash("Jump"));

        if (!_input.GetButton(ButtonInput.Jump) && _velocity.y > 0.0f)
        {
            _velocity.y = Mathf.Min(_velocity.y, Mathf.Sqrt(stats.GetSecondaryStat(SecondaryStatType.JumpHeight).GetValue()*-GambleConstants.GRAVITY));
        }

        if (_input.GetAxisValue(AxisInput.LeftStickX) > 0.5f)
        {
            normalizedHorizontalSpeed = 1;
            SetDirection(EntityDirection.Right);


        }
        else if (_input.GetAxisValue(AxisInput.LeftStickX) < -0.5f)
        {
            normalizedHorizontalSpeed = -1;
            SetDirection(EntityDirection.Left);


        }

        AttemptLedgeGrab();

        if((_input.GetAxisValue(AxisInput.LeftStickY) > 0.5f || _input.GetAxisValue(AxisInput.LeftStickY) < -0.5f) && _controller.collisionState.onLadder)
        {
            if (AttemptClimbLadder())
            {
                return;
            }
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

    public void Jump(bool leap = false)
    {
        if(movementState == PlayerMovementState.Jump || _controller.collisionState.becameGroundedThisFrame)
        {
            //Can't jump while jumping or in the air
            return;
        }



        _velocity.y = Mathf.Sqrt(2* stats.GetSecondaryStat(SecondaryStatType.JumpHeight).GetValue() * -GambleConstants.GRAVITY);
        movementState = PlayerMovementState.Jump;
        foreach(Ability ability in abilities)
        {
            if(ability is EffectOnJump jumpAbility)
            {
                jumpAbility.OnJump();
            }
        }
    }

    public override void Die()
    {
        base.Die();
        StopAllCoroutines();
        _attackManager.StopAllCoroutines();
        movementState = PlayerMovementState.Dead;
        StartCoroutine(Death());
    }



    public IEnumerator Death()
    {
        if(movementState == PlayerMovementState.Dead)
        {
            yield return 0;
        }

        if(ArenaBattleManager.instance.gameData.gameMode != GameMode.FreePlay)
        {
            playerData.lives--;
        }
        playerVersusUI.SetLives();

        GameManager.instance.RemovePlayerAtIndex(playerIndex);
        //Use this for when the player should be removed from updated stuff but we want to keep the body
        if (playerData.lives > 0)
        {
            StartCoroutine(ArenaBattleManager.instance.RespawnPlayer(playerIndex));
        }

        /*
        if (_input != null)
        {
            _input.Remove();
        }
        */


        Instantiate(deathEffect, transform.position, Quaternion.identity);

        playerLight.gameObject.SetActive(false);

        movementState = PlayerMovementState.Dead;

        _animator.Play(Animator.StringToHash("Dead"));

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
        {
            yield return null;
        }

        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {
            yield return null;
        }



        for(int i = particleEffects.Count-1; i >= 0; i--)
        {
            Destroy(particleEffects[i]);
        }

    }

    public IEnumerator Ressurect()
    {
        float resTimestamp = Time.time;
        int resCounter = 1;
        int resCountFull = 4;
        ShowFloatingText(resCounter + "", Color.blue);

        while (movementState == PlayerMovementState.Dead)
        {
            if (!_input.GetButton(ButtonInput.Jump)) {
                yield break;
            }

            if (Time.time >= resTimestamp + resCounter)
            {
                resCounter++;
                if (resCounter >= resCountFull)
                {
                    movementState = PlayerMovementState.Idle;
                    health.SetHealth(20);
                    playerLight.gameObject.SetActive(true);
                    isDead = false;
                    break;
                }

                ShowFloatingText(resCounter + "", Color.blue);
            }



            yield return null;
        }

    }

    public IEnumerator Rolling()
    {
        movementState = PlayerMovementState.Roll;
        float rollTimestamp = Time.time;
        //float speedMod = rollSpeed;
        SecondaryStatBonus speedBonus = new SecondaryStatBonus(SecondaryStatType.MoveSpeed, rollSpeed, StatModType.Mult);
        stats.GetSecondaryStat(SecondaryStatType.MoveSpeed).AddBonus(speedBonus);
        //movementSpeed *= speedMod;
        int rollDirection = GetDirection();
        hurtbox.colliderState = ColliderState.Closed;
        _controller.platformMask ^= rollMask;
        bool followUpAttack = false;

        while (movementState == PlayerMovementState.Roll)
        {
            _animator.Play(Animator.StringToHash("Roll"));
            _animator.speed = rollSpeed;

            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
            {
                yield return null;
            }

            //This checks if the animation has completed one cycle, and won't progress until it has
            //This allows for the animator speed to be adjusted by the "attack speed"
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                normalizedHorizontalSpeed = rollDirection;
                /*
                if(_input.GetButtonDown(ButtonInput.LightAttack))
                {
                    followUpAttack = true;
                }
                */
                yield return null;
            }
            

            stats.GetSecondaryStat(SecondaryStatType.MoveSpeed).RemoveBonus(speedBonus);
            _animator.speed = 1;
            hurtbox.colliderState = ColliderState.Open;
            _controller.platformMask |= rollMask;

            movementState = PlayerMovementState.Idle;


            yield return null;
        }

        /*
        if (followUpAttack)
        {
            //_attackManager.ActivateAttack();
            _input.buttonInput[(int)ButtonInput.LightAttack] = true;
            _input.previousButtonInput[(int)ButtonInput.LightAttack] = false;

        }
        */
    }
    
    public void PickupItem(ItemObject item)
    {
        //Move this to an inventory thing i think
        if(item.item is Weapon weapon)
        {
            _equipmentManager.EquipItem(weapon);
        }

        item.Collect();

    }

    public List<ItemObject> CheckForItems()
    {
        List<ItemObject> itemsFound = new List<ItemObject>();

        List<Collider2D> colliders = new List<Collider2D>();
        BoxCollider2D box = _controller.boxCollider;
        //box.size *= 2;
        Physics2D.OverlapCollider(_controller.boxCollider, itemFilter, colliders);
        //box.size /= 2;

        for (int i = 0; i < colliders.Count; i++)
        {
            ItemObject item = colliders[i].GetComponent<ItemObject>();

            if(item != null)
            {
                itemsFound.Add(item);
            }

        }

        return itemsFound;
    }

    public override void SetDirection(EntityDirection dir)
    {
        if (movementState == PlayerMovementState.Attacking || movementState == PlayerMovementState.Roll)
        {
            return;
        }


            transform.localScale = new Vector3((int)dir * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            _attackManager.rangedWeaponObject.transform.localScale = new Vector3((int)dir * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        
    }
}
