#define DEBUG_CC2D_RAYS
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

[RequireComponent( typeof( BoxCollider2D ), typeof( Rigidbody2D ) )]
public class PhysicsBody2D : MonoBehaviour
{
	#region internal types

	struct CharacterRaycastOrigins
	{
		public Vector3 topLeft;
		public Vector3 bottomRight;
		public Vector3 bottomLeft;
        public Vector3 topRight;
        public Vector3 ledgeGrabOffset;
	}
    [System.Serializable]
	public class CharacterCollisionState2D
	{
        //AKA things that you are moving
        public List<PhysicsBody2D> collidingBodies = new List<PhysicsBody2D>();
		public bool right;
		public bool left;
		public bool above;
		public bool below;
        public bool pushRight;
		public bool pushLeft;
		public bool pushAbove;
		public bool pushBelow;
        public bool groundRight;
        public bool groundLeft;
        public bool groundAbove;
        public bool groundBelow;
        public bool becameGroundedThisFrame;
		public bool wasGroundedLastFrame;
		public bool movingDownSlope;
		public float slopeAngle;
        public bool canGrabLedge;
        public bool onOneWayPlatform;
        public Vector3 ledgeGrabPosition;
        public Vector2[] rays = new Vector2[4];
        public bool onLadder;
        public Vector3 ladderOffset = Vector3.zero;
        public bool spiked = false;
        public bool hasPassenger = false;

		public bool hasCollision()
		{
			return below || right || left || above;
		}


		public void reset()
		{
			right = left = above = below = pushRight = pushLeft = pushAbove = pushBelow = groundRight = groundLeft = groundAbove = groundBelow
                = becameGroundedThisFrame = movingDownSlope = canGrabLedge = onLadder = onOneWayPlatform = spiked = false;
			slopeAngle = 0f;
            ledgeGrabPosition = Vector3.zero;
            ladderOffset = Vector3.zero;

            Vector2[] rays = new Vector2[4];
            //collidingBodies = new List<PhysicsBody2D>();
    }


    public override string ToString()
		{
			return string.Format( "[CharacterCollisionState2D] r: {0}, l: {1}, a: {2}, b: {3}, movingDownSlope: {4}, angle: {5}, wasGroundedLastFrame: {6}, becameGroundedThisFrame: {7}",
			                     right, left, above, below, movingDownSlope, slopeAngle, wasGroundedLastFrame, becameGroundedThisFrame );
		}
	}

    #endregion

    #region events, properties and fields

    public event Action<RaycastHit2D> onControllerCollidedEvent;
	public event Action<Collider2D> onTriggerEnterEvent;
	public event Action<Collider2D> onTriggerStayEvent;
	public event Action<Collider2D> onTriggerExitEvent;


	/// <summary>
	/// when true, one way platforms will be ignored when moving vertically for a single frame
	/// </summary>
	public bool ignoreOneWayPlatformsThisFrame;
    public bool ignoreGravity;

	[SerializeField]
	[Range( 0.001f, 0.3f )]
	float _skinWidth = 0.02f;
	/// <summary>
	/// defines how far in from the edges of the collider rays are cast from. If cast with a 0 extent it will often result in ray hits that are
	/// not desired (for example a foot collider casting horizontally from directly on the surface can result in a hit)
	/// </summary>
	public float skinWidth
	{
		get { return _skinWidth; }
		set
		{
			_skinWidth = value;
			recalculateDistanceBetweenRays();
		}
	}


	/// <summary>
	/// mask with all layers that this body should treat as a ground/wall/roof
	/// </summary>
	public LayerMask platformMask = 0;

	/// <summary>
	/// mask with all layers that trigger events should fire when intersected
	/// </summary>
	public LayerMask triggerMask = 0;

    public LayerMask ladderMask = 0;

    /// <summary>
    /// mask with all layers that should act as one-way platforms. Note that one-way platforms should always be EdgeCollider2Ds. This is because it does not support being
    /// updated anytime outside of the inspector for now.
    /// </summary>
    [SerializeField]
	LayerMask oneWayPlatformMask = 0;
    [SerializeField]
    LayerMask spikeMask = 0;

    /// <summary>
    /// the max slope angle that the CC2D can climb
    /// </summary>
    /// <value>The slope limit.</value>
    [Range( 0f, 90f )]
	public float slopeLimit = 30f;

	/// <summary>
	/// the threshold in the change in vertical movement between frames that constitutes jumping
	/// </summary>
	/// <value>The jumping threshold.</value>
	public float jumpingThreshold = 0.07f;


	/// <summary>
	/// curve for multiplying speed based on slope (negative = down slope and positive = up slope)
	/// </summary>
	public AnimationCurve slopeSpeedMultiplier = new AnimationCurve( new Keyframe( -90f, 1.5f ), new Keyframe( 0f, 1f ), new Keyframe( 90f, 0f ) );

	[Range( 2, 20 )]
	public int totalHorizontalRays = 8;
	[Range( 2, 20 )]
	public int totalVerticalRays = 4;


	/// <summary>
	/// this is used to calculate the downward ray that is cast to check for slopes. We use the somewhat arbitrary value 75 degrees
	/// to calculate the length of the ray that checks for slopes.
	/// </summary>
	float _slopeLimitTangent = Mathf.Tan( 75f * Mathf.Deg2Rad );


	[HideInInspector][NonSerialized]
	public new Transform transform;
	[HideInInspector][NonSerialized]
	public BoxCollider2D boxCollider;
	[HideInInspector][NonSerialized]
	public Rigidbody2D rigidBody2D;

	//[HideInInspector][NonSerialized]
	public CharacterCollisionState2D collisionState = new CharacterCollisionState2D();
	[HideInInspector][NonSerialized]
	public Vector3 velocity;
	public bool isGrounded { get { return collisionState.below; } }
    //This pretty much means not pushable right?
    public bool isKinematic = false; 
    public bool isPlatform = false;
    public List<PhysicsBody2D> riders = new List<PhysicsBody2D>();
    //public bool isPushable = false;

	const float kSkinWidthFloatFudgeFactor = 0.001f;

	#endregion


	/// <summary>
	/// holder for our raycast origin corners (TR, TL, BR, BL)
	/// </summary>
	CharacterRaycastOrigins _raycastOrigins;

	/// <summary>
	/// stores our raycast hit during movement
	/// </summary>
	RaycastHit2D[] hits;

	/// <summary>
	/// stores any raycast hits that occur this frame. we have to store them in case we get a hit moving
	/// horizontally and vertically so that we can send the events after all collision state is set
	/// </summary>
	List<RaycastHit2D> _raycastHitsThisFrame = new List<RaycastHit2D>( 2 );
    List<RaycastHit2D> _raycastPushHitsThisFrame = new List<RaycastHit2D>(2);

    // horizontal/vertical movement data
    float _verticalDistanceBetweenRays;
	float _horizontalDistanceBetweenRays;

	// we use this flag to mark the case where we are travelling up a slope and we modified our delta.y to allow the climb to occur.
	// the reason is so that if we reach the end of the slope we can make an adjustment to stay grounded
	bool _isGoingUpSlope = false;


	#region Monobehaviour

	void Awake()
	{
		// add our one-way platforms to our normal platform mask so that we can land on them from above
		platformMask |= oneWayPlatformMask;

		// cache some components
		transform = GetComponent<Transform>();
		boxCollider = GetComponent<BoxCollider2D>();
		rigidBody2D = GetComponent<Rigidbody2D>();

		// here, we trigger our properties that have setters with bodies
		skinWidth = _skinWidth;

		// we want to set our CC2D to ignore all collision layers except what is in our triggerMask
		for( var i = 0; i < 32; i++ )
		{
			// see if our triggerMask contains this layer and if not ignore it
			if( ( triggerMask.value & 1 << i ) == 0 )
				Physics2D.IgnoreLayerCollision( gameObject.layer, i );
		}
	}


	public void OnTriggerEnter2D( Collider2D col )
	{
		if( onTriggerEnterEvent != null )
			onTriggerEnterEvent( col );

    }


	public void OnTriggerStay2D( Collider2D col )
	{
		if( onTriggerStayEvent != null )
			onTriggerStayEvent( col );


    }


    public void OnTriggerExit2D( Collider2D col )
	{
		if( onTriggerExitEvent != null )
			onTriggerExitEvent( col );

    }

	#endregion


	[System.Diagnostics.Conditional( "DEBUG_CC2D_RAYS" )]
	void DrawRay( Vector3 start, Vector3 dir, Color color )
	{
		Debug.DrawRay( start, dir, color );
	}


	#region Public

	/// <summary>
	/// attempts to move the character to position + deltaMovement. Any colliders in the way will cause the movement to
	/// stop when run into.
	/// </summary>
	/// <param name="deltaMovement">Delta movement.</param>
	public void move()
	{
        //Apply Gravity
        if (!ignoreGravity)
        {
            velocity.y += GambleConstants.GRAVITY * Time.deltaTime;
        }

        Vector3 deltaMovement = (velocity * Time.deltaTime);
        // save off our current grounded state which we will use for wasGroundedLastFrame and becameGroundedThisFrame

        collisionState.wasGroundedLastFrame = collisionState.below;

        // clear our state
        collisionState.reset();
        _raycastHitsThisFrame.Clear();
        _raycastPushHitsThisFrame.Clear();
        _isGoingUpSlope = false;
        
		primeRaycastOrigins();


		// first, we check for a slope below us before moving
		// only check slopes if we are going down and grounded
		//if( deltaMovement.y < 0f && collisionState.wasGroundedLastFrame )
		//	handleVerticalSlope( ref deltaMovement );

		// now we check movement in the horizontal dir
		if( deltaMovement.x != 0f )
			moveHorizontally( ref deltaMovement );

		// next, check movement in the vertical dir
		if( deltaMovement.y != 0f )
			moveVertically( ref deltaMovement );


        checkLadder(deltaMovement);
        // move then update our state
        deltaMovement.z = 0;
		transform.Translate( deltaMovement, Space.World );

        List<PhysicsBody2D> tempRiders = new List<PhysicsBody2D>();
        tempRiders.AddRange(riders);

        foreach(PhysicsBody2D rider in tempRiders)
        {
            if(rider != null && riders.Contains(rider))
            {
                rider.pushMove(deltaMovement);
            }
        }

        riders.Clear();

		// only calculate velocity if we have a non-zero deltaTime
		if( Time.deltaTime > 0f )
			velocity = deltaMovement / Time.deltaTime;

		// set our becameGrounded state based on the previous and current collision state
		if( !collisionState.wasGroundedLastFrame && collisionState.below )
			collisionState.becameGroundedThisFrame = true;

		// if we are going up a slope we artificially set a y velocity so we need to zero it out here
		if( _isGoingUpSlope )
			velocity.y = 0;

		// send off the collision events if we have a listener
		if( onControllerCollidedEvent != null )
		{
			for( var i = 0; i < _raycastHitsThisFrame.Count; i++ )
				onControllerCollidedEvent( _raycastHitsThisFrame[i] );
		}

        if( onControllerCollidedEvent != null)
        {
            //blah blah blah

        }

        /*
        if (!isKinematic)
        {
            foreach (RaycastHit2D hit in _raycastHitsThisFrame)
            {
                //Check for platforms
                PhysicsBody2D body = hit.transform.GetComponent<PhysicsBody2D>();

                if (body != null && body.isKinematic && !collisionState.collidingBodies.Contains(body))
                {
                    collisionState.platform = body;
                    break;
                }
            }
        }

        if(collisionState.platform != null)
        {
            transform.parent = collisionState.platform.transform;
        } else
        {
            transform.parent = null;
        }
        */
        /*
        if(!isKinematic)
        {
            foreach (RaycastHit2D hit in _raycastHitsThisFrame)
            {
                //Check for platforms
                PhysicsBody2D body = hit.transform.GetComponent<PhysicsBody2D>();

                if (body != null && !collisionState.collidingBodies.Contains(body))
                {
                    body.collisionState.collidingBodies.Add(this);
                }
            }
        }


        foreach(PhysicsBody2D body in collisionState.collidingBodies)
        {
            body.transform.Translate(deltaMovement, Space.World);

        }

        collisionState.collidingBodies.Clear();

        /*
         * 
        //foreach(PhysicsBody2D body in collisionState.)
    }
        */

    }


    public void pushMove(Vector3 deltaMovement)
    {

        // clear our state
        //collisionState.reset();
        //_isGoingUpSlope = false;

        primeRaycastOrigins();


        // first, we check for a slope below us before moving
        // only check slopes if we are going down and grounded
        //if( deltaMovement.y < 0f && collisionState.wasGroundedLastFrame )
        //	handleVerticalSlope( ref deltaMovement );

        // now we check movement in the horizontal dir
        if (deltaMovement.x != 0f)
            moveHorizontally(ref deltaMovement);

        // next, check movement in the vertical dir
        if (deltaMovement.y != 0f)
            moveVertically(ref deltaMovement);


        checkLadder(deltaMovement);
        // move then update our state
        deltaMovement.z = 0;
        transform.Translate(deltaMovement, Space.World);


        // set our becameGrounded state based on the previous and current collision state
        if (!collisionState.wasGroundedLastFrame && collisionState.below)
            collisionState.becameGroundedThisFrame = true;

        // send off the collision events if we have a listener
        if (onControllerCollidedEvent != null)
        {
            for (var i = 0; i < _raycastHitsThisFrame.Count; i++)
                onControllerCollidedEvent(_raycastHitsThisFrame[i]);
        }



        
    }

    /// <summary>
    /// moves directly down until grounded
    /// </summary>
    /// 
    /// 
    /*
    public void warpToGrounded()
	{
		do
		{
			move( new Vector3( 0, -1f, 0 ) );
		} while( !isGrounded );
	}
    */

	/// <summary>
	/// this should be called anytime you have to modify the BoxCollider2D at runtime. It will recalculate the distance between the rays used for collision detection.
	/// It is also used in the skinWidth setter in case it is changed at runtime.
	/// </summary>
	public void recalculateDistanceBetweenRays()
	{
		// figure out the distance between our rays in both directions
		// horizontal
		var colliderUseableHeight = boxCollider.size.y * Mathf.Abs( transform.localScale.y ) - ( 2f * _skinWidth );
		_verticalDistanceBetweenRays = colliderUseableHeight / ( totalHorizontalRays - 1 );

		// vertical
		var colliderUseableWidth = boxCollider.size.x * Mathf.Abs( transform.localScale.x ) - ( 2f * _skinWidth );
		_horizontalDistanceBetweenRays = colliderUseableWidth / ( totalVerticalRays - 1 );
	}

	#endregion


	#region Movement Methods

	/// <summary>
	/// resets the raycastOrigins to the current extents of the box collider inset by the skinWidth. It is inset
	/// to avoid casting a ray from a position directly touching another collider which results in wonky normal data.
	/// </summary>
	/// <param name="futurePosition">Future position.</param>
	/// <param name="deltaMovement">Delta movement.</param>
	void primeRaycastOrigins()
	{
		// our raycasts need to be fired from the bounds inset by the skinWidth
        //boxCollider.
		var modifiedBounds = boxCollider.bounds;
		modifiedBounds.Expand( -2f * _skinWidth );

		_raycastOrigins.topLeft = new Vector2( modifiedBounds.min.x, modifiedBounds.max.y );
        _raycastOrigins.ledgeGrabOffset = new Vector2(modifiedBounds.min.x, modifiedBounds.max.y + 2);
        _raycastOrigins.topRight = new Vector2(modifiedBounds.max.x, modifiedBounds.max.y);
        _raycastOrigins.bottomRight = new Vector2( modifiedBounds.max.x, modifiedBounds.min.y );
		_raycastOrigins.bottomLeft = modifiedBounds.min;

        recalculateDistanceBetweenRays();

    }


    /// <summary>
    /// we have to use a bit of trickery in this one. The rays must be cast from a small distance inside of our
    /// collider (skinWidth) to avoid zero distance rays which will get the wrong normal. Because of this small offset
    /// we have to increase the ray distance skinWidth then remember to remove skinWidth from deltaMovement before
    /// actually moving the player
    /// </summary>
    void moveHorizontally(ref Vector3 deltaMovement)
    {
        var isGoingRight = deltaMovement.x > 0;
        var rayDistance = Mathf.Abs(deltaMovement.x) + _skinWidth;
        var rayDirection = isGoingRight ? Vector2.right : -Vector2.right;
        var initialRayOrigin = isGoingRight ? _raycastOrigins.bottomRight : _raycastOrigins.bottomLeft;

        bool grabRayOne = false;
        bool grabRayTwo = false;

        for (var i = 0; i < totalHorizontalRays; i++)
        {
            var ray = new Vector2(initialRayOrigin.x, initialRayOrigin.y + i * _verticalDistanceBetweenRays);

            //This will display the rays being drawn
            //DrawRay(ray, rayDirection * rayDistance, Color.red);

            // if we are grounded we will include oneWayPlatforms only on the first ray (the bottom one). this will allow us to
            // walk up sloped oneWayPlatforms
            /*
            if ( i == 0 && collisionState.wasGroundedLastFrame )
				_raycastHit = Physics2D.Raycast( ray, rayDirection, rayDistance, platformMask );
			else
            */

        hits = Physics2D.RaycastAll(ray, rayDirection, rayDistance, platformMask & ~oneWayPlatformMask);

            for (int h = 0; h < hits.Length; h++)
            {
                if (hits[h].transform.gameObject == gameObject)
                {
                    continue;
                }

                if (hits[h])
                {
                    //DrawRay(ray, rayDirection * (rayDistance + 1), Color.red);
                    //_raycastHit.point
                    Tilemap map = hits[h].collider.GetComponent<Tilemap>();
                    if (map != null)
                    {
                        if (i == totalHorizontalRays - 1)
                        {
                            grabRayOne = true;
                        }

                        if (i == totalHorizontalRays - 2)
                        {

                            //_raycastHit.point

                            grabRayTwo = true;
                            //TileBase tile = map.GetTile(map.layoutGrid.WorldToCell(_raycastHit.point));
                            collisionState.ledgeGrabPosition = map.layoutGrid.WorldToCell(hits[h].point);


                        }

                        deltaMovement.x = hits[h].point.x - ray.x;

                        // set our new deltaMovement and recalculate the rayDistance taking it into account
                        rayDistance = Mathf.Abs(deltaMovement.x);

                        // remember to remove the skinWidth from our deltaMovement
                        if (isGoingRight)
                        {
                            deltaMovement.x -= _skinWidth;
                            collisionState.right = true;
                            collisionState.groundRight = true;
                        }
                        else
                        {
                            deltaMovement.x += _skinWidth;
                            collisionState.left = true;
                            collisionState.groundLeft = true;

                        }

                        _raycastHitsThisFrame.Add(hits[h]);
                    }



                    PhysicsBody2D other = hits[h].transform.GetComponent<PhysicsBody2D>();


                    if (other != null)
                    {

                        if (other.isKinematic || (isGoingRight && other.collisionState.right) || (!isGoingRight && other.collisionState.left))
                        {
                            // set our new deltaMovement and recalculate the rayDistance taking it into account
                            deltaMovement.x = hits[h].point.x - ray.x;
                            rayDistance = Mathf.Abs(deltaMovement.x);

                            // remember to remove the skinWidth from our deltaMovement
                            if (isGoingRight)
                            {
                                deltaMovement.x -= _skinWidth;
                                collisionState.right = true;
                                collisionState.pushRight = true;
                            }
                            else
                            {
                                deltaMovement.x += _skinWidth;
                                collisionState.left = true;
                                collisionState.pushLeft = true;

                            }

                        }
                        else
                        {

                            if (isGoingRight)
                            {

                                //deltaMovement.x -= _skinWidth;
                                collisionState.right = true;
                                collisionState.pushRight = true;
                                other.collisionState.left = true;
                                other.collisionState.pushLeft = true;

                            }
                            else
                            {
                                //deltaMovement.x += _skinWidth;
                                collisionState.left = true;
                                collisionState.pushLeft = true;
                                other.collisionState.right = true;
                                other.collisionState.pushRight = true;


                            }
                            //float pushAmount = (velocity.y - other.velocity.y) * Time.deltaTime;
                            //other.transform.Translate(Vector2.right * deltaMovement.x);
                            other.pushMove(Vector2.right * deltaMovement.x);

                        }


                        // we add a small fudge factor for the float operations here. if our rayDistance is smaller
                        // than the width + fudge bail out because we have a direct impact
                        if (rayDistance < _skinWidth + kSkinWidthFloatFudgeFactor)
                            break;

                    }


                }
            }


        }

        if (!grabRayOne && grabRayTwo)
        {
            collisionState.canGrabLedge = true;
        }
    }

    void checkLadder(Vector3 deltaMovement )
	{
        //This probably has a bunch of unneccesary stuff, but its actually working for now so this is what im going with

		var isGoingUp = deltaMovement.y < 0;
		var rayDistance = Mathf.Abs( deltaMovement.y ) + _skinWidth;
		var rayDirection = isGoingUp ? Vector2.up : -Vector2.up;
		var initialRayOrigin = isGoingUp ? _raycastOrigins.topLeft : _raycastOrigins.bottomLeft;

		initialRayOrigin.x += deltaMovement.x;

		var ray = new Vector2( initialRayOrigin.x + _horizontalDistanceBetweenRays*totalVerticalRays/2, initialRayOrigin.y );

		DrawRay( ray, rayDirection * 0.1f, Color.yellow );
		RaycastHit2D ladderHit = Physics2D.Raycast( ray, rayDirection, rayDistance, ladderMask );

        if (ladderHit)
        {
            Tilemap map = ladderHit.collider.GetComponent<Tilemap>();
            if (map != null)
            {
                //TileBase tile = map.GetTile(map.layoutGrid.WorldToCell(_raycastHit.point));
                collisionState.ladderOffset = map.layoutGrid.WorldToCell(ladderHit.point) + Vector3.right * 0.5f;
                collisionState.onLadder = true;
                
            }
        }
        
	}

	void moveVertically( ref Vector3 deltaMovement )
	{
		var isGoingUp = deltaMovement.y > 0;
        var isGoingDown = deltaMovement.y < 0;
        var rayDistance = Mathf.Abs( deltaMovement.y ) + _skinWidth;
		var rayDirection = isGoingUp ? Vector2.up : -Vector2.up;
		var initialRayOrigin = isGoingUp ? _raycastOrigins.topLeft : _raycastOrigins.bottomLeft;

		// apply our horizontal deltaMovement here so that we do our raycast from the actual position we would be in if we had moved
		initialRayOrigin.x += deltaMovement.x;

        //Detect for "ground"
		// if we are moving up, we should ignore the layers in oneWayPlatformMask
		var mask = platformMask;
		if( ( isGoingUp && !collisionState.wasGroundedLastFrame ) || ignoreOneWayPlatformsThisFrame )
			mask &= ~oneWayPlatformMask;

		for( var i = 0; i < totalVerticalRays; i++ )
		{
			var ray = new Vector2( initialRayOrigin.x + i * _horizontalDistanceBetweenRays, initialRayOrigin.y );

			//DrawRay( ray, rayDirection *0.1f, Color.red );


            hits = Physics2D.RaycastAll( ray, rayDirection, rayDistance, mask );


            for (int h = 0; h < hits.Length; h++)
            {
                if (hits[h].transform.gameObject == gameObject)
                {
                    continue;
                }

                if (hits[h])
                {
                    //DrawRay(ray, rayDirection * (rayDistance + 1), Color.red);
                    //_raycastHit.point
                    Tilemap map = hits[h].collider.GetComponent<Tilemap>();
                    if (map != null)
                    {


                        // set our new deltaMovement and recalculate the rayDistance taking it into account
                        deltaMovement.y = hits[h].point.y - ray.y;
                        rayDistance = Mathf.Abs(deltaMovement.y);

                        // remember to remove the skinWidth from our deltaMovement
                        if (isGoingUp)
                        {
                            deltaMovement.y -= _skinWidth;
                            collisionState.above = true;
                            collisionState.groundAbove = true;

                        }
                        else
                        {
                            deltaMovement.y += _skinWidth;
                            collisionState.below = true;
                            collisionState.groundBelow = true;
                            if (hits[h].transform.gameObject.layer == LayerMask.NameToLayer("OneWayPlatform"))
                            {
                                collisionState.onOneWayPlatform = true;
                            }

                        }

                        // this is a hack to deal with the top of slopes. if we walk up a slope and reach the apex we can get in a situation
                        // where our ray gets a hit that is less then skinWidth causing us to be ungrounded the next frame due to residual velocity.
                        if (!isGoingUp && deltaMovement.y > 0.00001f)
                            _isGoingUpSlope = true;
                    }




                    // we add a small fudge factor for the float operations here. if our rayDistance is smaller
                    // than the width + fudge bail out because we have a direct impact
                    /*
                    if (rayDistance < _skinWidth + kSkinWidthFloatFudgeFactor)
                        break;
                    */
                    PhysicsBody2D other = hits[h].transform.GetComponent<PhysicsBody2D>();

                    if (other != null)
                    {
                        if(other.isPlatform && !isKinematic && isGoingDown)
                        {
                            deltaMovement.y = hits[h].point.y - ray.y;
                            rayDistance = Mathf.Abs(deltaMovement.y);

                            deltaMovement.y += _skinWidth;
                            collisionState.below = true;
                            collisionState.pushBelow = true;

                            if (!isGoingUp && deltaMovement.y > 0.00001f)
                                _isGoingUpSlope = true;

                            if(!other.riders.Contains(this))
                            other.riders.Add(this);
                        }
                        else
                        {
                            if (other.isKinematic || (isGoingUp && other.collisionState.above) || (!isGoingUp && other.collisionState.below))
                            {
                                // set our new deltaMovement and recalculate the rayDistance taking it into account
                                deltaMovement.y = hits[h].point.y - ray.y;
                                rayDistance = Mathf.Abs(deltaMovement.y);

                                // remember to remove the skinWidth from our deltaMovement
                                if (isGoingUp)
                                {
                                    deltaMovement.y -= _skinWidth;
                                    collisionState.above = true;
                                    collisionState.pushAbove = true;
                                }
                                else
                                {
                                    deltaMovement.y += _skinWidth;
                                    collisionState.below = true;
                                    collisionState.pushBelow = true;

                                }

                                if (!isGoingUp && deltaMovement.y > 0.00001f)
                                    _isGoingUpSlope = true;
                            }
                            else
                            {

                                if (isGoingUp)
                                {

                                    //deltaMovement.x -= _skinWidth;
                                    collisionState.above = true;
                                    collisionState.pushAbove = true;
                                    other.collisionState.below = true;
                                    other.collisionState.pushBelow = true;
                                }
                                else
                                {
                                    //deltaMovement.x += _skinWidth;
                                    collisionState.below = true;
                                    collisionState.pushBelow = true;
                                    other.collisionState.above = true;
                                    other.collisionState.pushAbove = true;


                                }
                                //float pushAmount = (velocity.y - other.velocity.y) * Time.deltaTime;
                                //other.transform.Translate(Vector2.right * deltaMovement.x);
                                //other.pushMove(Vector2.up * deltaMovement.y);

                            }


                            // we add a small fudge factor for the float operations here. if our rayDistance is smaller
                            // than the width + fudge bail out because we have a direct impact
                            if (rayDistance < _skinWidth + kSkinWidthFloatFudgeFactor)
                                break;
                        }

                        

                    }


                }

                _raycastHitsThisFrame.Add(hits[h]);

            }

            if (isGoingDown && !collisionState.below)
            {
                RaycastHit2D[] spikes = Physics2D.RaycastAll(ray, rayDirection, rayDistance, spikeMask);

                if (spikes.Length > 0)
                {
                    collisionState.spiked = true;
                    
                }
            }




        }
        
        

        
        
    }


	#endregion

}
