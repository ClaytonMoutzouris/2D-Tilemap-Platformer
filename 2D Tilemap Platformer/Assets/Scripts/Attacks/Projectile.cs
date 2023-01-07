using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsBody2D))]
[RequireComponent(typeof(AttackObject))]
public class Projectile : MonoBehaviour
{
    public ProjectileData projectileData;

    public Vector2 direction;
    float startTime = 0;

    public AttackObject _attackObject;

    //Physics things
    protected PhysicsBody2D _controller;
    public LayerMask baseLayermask;
    //Might want to create a seperate projectile class for "boomerangs", but modularity has its merits aswell
    bool returning = false;

    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public bool bouncedLastFrame = false;


    public void Awake()
    {
        _controller = GetComponent<PhysicsBody2D>();
        _attackObject = GetComponent<AttackObject>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        baseLayermask = _controller.platformMask;
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
        if (projectileData.projectileFlags.GetFlag(ProjectileFlagType.IsAngled).GetValue())
        {
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

            transform.eulerAngles = new Vector3(0, 0, -angle);
        }

        /*
        if(angle < 0)
        {
            spriteRenderer.flipY = true;
            spriteRenderer.flipX = true;

        }
        */

        if (!projectileData.projectileFlags.GetFlag(ProjectileFlagType.IgnoreGravity).GetValue())
        {
            _controller.velocity.y = Mathf.Sqrt(projectileData.projSpeed *direction.normalized.y * -GambleConstants.GRAVITY);
            //Debug.Log("Velocity " + _velocity);
            _controller.velocity.x = direction.normalized.x * projectileData.projSpeed;

        }
        else
        {
            _controller.velocity.y = direction.normalized.y * projectileData.projSpeed;
            _controller.velocity.x = direction.normalized.x * projectileData.projSpeed;
        }

    }

    public virtual void SetToOwnerDirection(EntityDirection dir)
    {
        if (transform.localScale.x < 0f && dir == EntityDirection.Right || transform.localScale.x > 0f && dir == EntityDirection.Left)
        {
            transform.localScale = new Vector3((int)dir * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public void CheckBounce()
    {
        if (!bouncedLastFrame)
        {
            if (_controller.collisionState.above)
            {
                direction.y *= -1;
            }
            else if (_controller.collisionState.below)
            {
                direction.y *= -1;
                if (!projectileData.projectileFlags.GetFlag(ProjectileFlagType.IgnoreGravity).GetValue())
                {
                    _controller.velocity.y = Mathf.Sqrt(projectileData.projSpeed * direction.y * -GambleConstants.GRAVITY);
                }
            }

            if (_controller.collisionState.left || _controller.collisionState.right)
            {
                direction.x *= -1;

            }
            _attackObject.ClearHits();
            bouncedLastFrame = true;
            _controller.velocity.y = direction.normalized.y * projectileData.projSpeed;
            _controller.velocity.x = direction.normalized.x * projectileData.projSpeed;
        }
        else
        {
            bouncedLastFrame = false;
        }
    }

    protected void Update()
    {

        //_velocity.x = direction.normalized.x * projectileData.projSpeed;

        if(projectileData.projectileFlags.GetFlag(ProjectileFlagType.Bounce).GetValue())
        {
            CheckBounce();
        }
        else if (!projectileData.projectileFlags.GetFlag(ProjectileFlagType.Slippery).GetValue())
        {
            if (_controller.collisionState.below || _controller.collisionState.left || _controller.collisionState.right)
            {
                _controller.velocity.x = 0;
            }
        }



        if (projectileData.projectileFlags.GetFlag(ProjectileFlagType.Boomerang).GetValue())
        {
            Boomerang(ref _controller.velocity);
        }
        
        if(projectileData.projectileFlags.GetFlag(ProjectileFlagType.IsAngled).GetValue())
        {
            Vector2 dir = _controller.velocity.normalized;
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, -angle);
        }

        _controller.move();



        if (_controller.collisionState.hasCollision())
        {
            if (projectileData.projectileFlags.GetFlag(ProjectileFlagType.DestroyOnGround).GetValue())
            {
                DestroyProjectile();
            } 
        }

        if (startTime + projectileData.lifeTime <= Time.time)
        {
            DestroyProjectile();
        }

        //Destroy the object when it makes a collision unless it has piercing
        if (!projectileData.projectileFlags.GetFlag(ProjectileFlagType.Piercing).GetValue() && _attackObject.hits.Count > 0)
        {
            DestroyProjectile();
        }

    }

    public virtual void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    public void Boomerang(ref Vector3 vel)
    {
        _controller.velocity.y = direction.normalized.y * projectileData.projSpeed;
        _controller.velocity.x = direction.normalized.x * projectileData.projSpeed;

        float returnSpeed = projectileData.elasticity * (Time.time - startTime);
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

        projectileData.projSpeed = wep.GetStatValue(WeaponAttributesType.ProjectileSpeed);
        projectileData.lifeTime = wep.GetStatValue(WeaponAttributesType.ProjectileLifeTime);
        projectileData.projectileFlags.AddBonuses(wep.projectileBonuses);
        projectileData.image = wep.sprite;
        spriteRenderer.color = wep.color;

        if (projectileData.projectileFlags.GetFlag(ProjectileFlagType.IgnoreGround).GetValue())
        {
            _controller.platformMask = new LayerMask();
        }
        else
        {
            _controller.platformMask = baseLayermask;
        }


        _controller.ignoreGravity = projectileData.projectileFlags.GetFlag(ProjectileFlagType.IgnoreGravity).GetValue();

    }

    public void SetData(ProjectileData data)
    {
        projectileData = Instantiate(data);
        projectileData.SetBaseFlags();

        spriteRenderer.sprite = data.image;
        animator.runtimeAnimatorController = data.animator;

        _attackObject.hitbox.size = data.size;
        _attackObject.attackData = data.attackData;
        _attackObject.contactFilter = data.contactFilter;
        
        foreach(ParticleSystem effect in data.visualEffects)
        {
            ParticleSystem temp = Instantiate(effect, transform);
        }

        if (projectileData.projectileFlags.GetFlag(ProjectileFlagType.IgnoreGround).GetValue())
        {
            _controller.platformMask = new LayerMask();
        } else
        {
            _controller.platformMask = baseLayermask;
        }

        _controller.ignoreGravity = projectileData.projectileFlags.GetFlag(ProjectileFlagType.IgnoreGravity).GetValue();

    }
}
