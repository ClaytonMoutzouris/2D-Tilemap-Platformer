using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : Entity
{
    public float maxJumpHeight = 3f;

    public PhysicsBody2D _controller;
    public Vector3 mOldPosition;
    public Entity target;
    public Sightbox sight;
    public List<AttackObject> attackObjects;

    public int normalizedHorizontalSpeed = 0;


    protected override void Awake()
    {
        base.Awake();

        _controller = GetComponent<PhysicsBody2D>();
        sight = GetComponentInChildren<Sightbox>();

        //_controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;

        foreach(AttackObject attack in attackObjects)
        {
            attack.SetOwner(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (target!= null && target.health.IsDead())
        {
            target = null;
        }

        if (!knockedBack)
        {
            _controller.velocity.x = normalizedHorizontalSpeed * movementSpeed;
        }


        mOldPosition = transform.position;
        _controller.move();


    }

    public override void Die()
    {
        base.Die();
            Destroy(gameObject, 0.5f);
            //movementState = PlayerMovementState.Dead;
    }

    public void SearchForTarget()
    {

        if (sight == null || sight.inSight.Count == 0)
        {
            return;
        }

        target = sight.inSight[Random.Range(0, sight.inSight.Count)];

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


