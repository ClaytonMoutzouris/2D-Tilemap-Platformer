using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : CharacterEntity
{
    public float maxJumpHeight = 3f;

    public Vector3 mOldPosition;
    public Entity target;
    public List<AttackObject> attackObjects;

    public int normalizedHorizontalSpeed = 0;


    protected override void Awake()
    {
        base.Awake();

        _controller = GetComponent<PhysicsBody2D>();

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

        if (target != null && !target.isDead)
        {
            target = null;
        }

        if (!knockedBack)
        {
            float velXThisFrame = (normalizedHorizontalSpeed * stats.GetSecondaryStat(SecondaryStatType.MoveSpeed).GetValue() * 10) * Time.deltaTime;

            if (velXThisFrame > 0 && _controller.velocity.x > 0)
            {
                if (Mathf.Abs(_controller.velocity.x) < stats.GetSecondaryStat(SecondaryStatType.MoveSpeed).GetValue())
                {
                    _controller.velocity.x += velXThisFrame;
                }
                //otherwise do nothing

            }
            else if (velXThisFrame < 0 && _controller.velocity.x < 0)
            {
                if (Mathf.Abs(_controller.velocity.x) < stats.GetSecondaryStat(SecondaryStatType.MoveSpeed).GetValue())
                {
                    _controller.velocity.x += velXThisFrame;
                }

                //otherwise do nothing
            }
            else
            {
                _controller.velocity.x += velXThisFrame;

            }

            if(_controller.isGrounded && normalizedHorizontalSpeed == 0)
            {
                _controller.velocity.x = (Mathf.Pow((1 - GambleConstants.GROUND_FRICTION), Time.deltaTime)) * _controller.velocity.x;
            }
        }


        mOldPosition = transform.position;
        _controller.move();


    }

    public override void Die()
    {
        base.Die();
        Destroy(gameObject, 0.2f);
            //movementState = PlayerMovementState.Dead;
    }

    public void SearchForTarget()
    {

        if (sight == null || sight.entitiesInSight.Count == 0)
        {
            return;
        }

        target = sight.entitiesInSight[Random.Range(0, sight.entitiesInSight.Count)];

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


