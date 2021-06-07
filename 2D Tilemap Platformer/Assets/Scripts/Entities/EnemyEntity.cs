using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : Entity
{
    public float maxJumpHeight = 3f;

    public PhysicsBody2D _controller;
    public Vector3 mOldPosition;

    public Vector3 _velocity;

    public int normalizedHorizontalSpeed = 0;


    protected override void Awake()
    {
        base.Awake();

        _controller = GetComponent<PhysicsBody2D>();
        //_controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
    }

    // Update is called once per frame
    void Update()
    {
        _velocity.x = normalizedHorizontalSpeed * movementSpeed;

        if (!ignoreGravity)
            _velocity.y += GambleConstants.GRAVITY * Time.deltaTime;

        mOldPosition = transform.position;
        _controller.move(_velocity * Time.deltaTime);

        _velocity = _controller.velocity;

    }

    void onTriggerEnterEvent(Collider2D col)
    {
        //Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
    }


    void onTriggerExitEvent(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("OneWayPlatform"))
        {
            _controller.ignoreOneWayPlatformsThisFrame = false;
        }
    }
}


