using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsBody2D))]
[RequireComponent(typeof(AttackObject))]
public class Projectile : MonoBehaviour
{
    public bool pierce = false;
    public bool boomerang = false;
    public bool homing = false;
    public bool ignoreGround = false;
    public bool ignoreGravity = true;
    public bool isAngled = false;


    public Vector2 direction;
    public float lifeTime = 1;
    public float startTime = 0;

    public AttackObject _attackObject;

    //Physics things
    protected PhysicsBody2D _controller;
    public float projSpeed = 5;
    //Might want to create a seperate projectile class for "boomerangs", but modularity has its merits aswell
    public float elasticity = -0.5f;
    bool returning = false;

    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Vector3 _velocity;



    public void Awake()
    {
        _controller = GetComponent<PhysicsBody2D>();
        _attackObject = GetComponent<AttackObject>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Might need this later?
        /*
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
        */
    }

    private void Start()
    {
        startTime = Time.time;
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, -angle);
        if(!ignoreGravity)
        {
            _velocity.y = Mathf.Sqrt(projSpeed*direction.y * -GambleConstants.GRAVITY);
        }
    }

    protected void Update()
    {

        _velocity.x = direction.x * projSpeed;

        if (!ignoreGravity)
        {
            _velocity.y += GambleConstants.GRAVITY * Time.deltaTime;
        }

        
        if (boomerang)
        {
            Boomerang(ref _velocity);
        }
        
        if(isAngled)
        {
            Vector2 dir = _velocity.normalized;
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, -angle);
        }

        _controller.move(_velocity * Time.deltaTime);

        _velocity = _controller.velocity;

        if (_controller.collisionState.hasCollision() && !ignoreGround)
        {
            Destroy(gameObject);
        }

        if (startTime + lifeTime <= Time.time)
        {
            Destroy(gameObject);
        }

        //Destroy the object when it makes a collision unless it has piercing
        if(!pierce && _attackObject.hits.Count > 0)
        {
            Destroy(gameObject);
        }

    }

    public void Boomerang(ref Vector3 vel)
    {
        float returnSpeed = elasticity * (Time.time - startTime);
        vel = vel - vel * returnSpeed;

        if(!returning && returnSpeed >= 1)
        {
            returning = true;
            _attackObject.ClearHits();

        }
    }

    public virtual void SetFromWeapon(Weapon wep)
    {
        _attackObject.SetOwner(wep.owner);

        _attackObject.attackData = wep.GetAttackData();

        projSpeed = wep.GetStatValue(WeaponAttributesType.ProjectileSpeed);

    }
}
