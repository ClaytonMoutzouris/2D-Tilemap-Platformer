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

    public override void GetHit(AttackObject attack)
    {
        //Debug.Log(name + " was hit by attack " + attack + " for " + attack.attackData.Damage);
        base.GetHit(attack);

        Projectile proj = attack.GetComponent<Projectile>();
        Vector2 difference;

        if (proj)
        {
            difference = (transform.position - attack.transform.position).normalized;
        }
        else
        {
            difference = (transform.position - attack.owner.transform.position).normalized;
        }


        _velocity = difference * 5 + Vector2.up * Mathf.Sqrt(-GambleConstants.GRAVITY);


        if (health.currentHealth <= 0)
        {
            Debug.Log("Enemy died");
            Destroy(gameObject);
            //movementState = PlayerMovementState.Dead;
        }
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


