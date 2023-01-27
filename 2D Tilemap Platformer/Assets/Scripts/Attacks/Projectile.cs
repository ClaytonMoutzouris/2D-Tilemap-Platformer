using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileMovementType { Straight, Arc, Boomerang, Control, Homing, Laser, Static }

[RequireComponent(typeof(PhysicsBody2D))]
[RequireComponent(typeof(AttackObject))]
public class Projectile : MonoBehaviour
{
    public ProjectileData projectileData;

    public Vector2 direction;
    protected float startTime = 0;

    public AttackObject _attackObject;

    //Physics things
    protected PhysicsBody2D _controller;
    public LayerMask baseLayermask;
    //Might want to create a seperate projectile class for "boomerangs", but modularity has its merits aswell
    bool returning = false;

    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public bool bouncedLastFrame = false;
    public ProjectileMovementType movementType;
    public List<TriggeredEffect> triggeredEffects;

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


        if (!projectileData.projectileFlags.GetFlag(ProjectileFlagType.IgnoreGravity).GetValue())
        {

            if(direction.normalized.y > 0)
            {
                _controller.velocity.y = Mathf.Sqrt(direction.normalized.y * projectileData.projSpeed * -GambleConstants.GRAVITY);

            }
            else
            {
                _controller.velocity.y = direction.normalized.y * projectileData.projSpeed;
            }

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


    protected void Update()
    {

        HandleMovement();

    }

    public virtual void HandleMovement()
    {
        //_velocity.x = direction.normalized.x * projectileData.projSpeed;

        if (projectileData.projectileFlags.GetFlag(ProjectileFlagType.Bounce).GetValue())
        {
            CheckBounce();
        }

        if (!bouncedLastFrame && !projectileData.projectileFlags.GetFlag(ProjectileFlagType.Slippery).GetValue())
        {
            if (_controller.collisionState.below)
            {
                _controller.velocity.x = 0;
            }
        }

        switch(movementType)
        {
            case (ProjectileMovementType.Straight):
                StraightMovement();
                break;
             case (ProjectileMovementType.Arc):
                ArcMovement();
                break;
            case (ProjectileMovementType.Boomerang):
                Boomerang();
                break;
            case (ProjectileMovementType.Control):
                ControlMovement();
                break;
            case (ProjectileMovementType.Homing):
                //Not implemented
                break;
            case (ProjectileMovementType.Laser):
                //Not implemented
                break;
            case (ProjectileMovementType.Static):
                _controller.velocity = Vector3.zero;
                break;
        }

        if (projectileData.projectileFlags.GetFlag(ProjectileFlagType.IsAngled).GetValue())
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

    public void StraightMovement()
    {


    }

    public void ArcMovement()
    {


    }

    public void ControlMovement()
    {
        if (_attackObject.owner is PlayerController player)
        {
            Vector2 aim = player._input.GetRightStickAim().normalized;

            if (aim != Vector2.zero)
            {
                _controller.velocity.x += aim.x * projectileData.projSpeed;
                _controller.velocity.y += aim.y * projectileData.projSpeed;
            }
            else
            {

            }
        }


    }


    public virtual void DestroyProjectile()
    {
        foreach(TriggeredEffect effect in triggeredEffects)
        {
            effect.Trigger();
        }

        Destroy(gameObject);
    }

    public void Boomerang()
    {
        Vector2 vel = _controller.velocity;

        //_controller.velocity.y = direction.normalized.y * projectileData.projSpeed;
        //_controller.velocity.x = direction.normalized.x * projectileData.projSpeed;

        float returnSpeed = projectileData.elasticity * (Time.time - startTime);
        vel = vel - vel * returnSpeed;

        _controller.velocity = vel;

        if (!returning && returnSpeed >= 1)
        {
            returning = true;
            _attackObject.ClearHits();

        }
    }

    public void CheckBounce()
    {
        if (!bouncedLastFrame)
        {
            if (_controller.collisionState.groundAbove)
            {
                direction.y *= -1;
                _attackObject.ClearHits();
                bouncedLastFrame = true;
                _controller.velocity.y = direction.normalized.y * projectileData.projSpeed;
                _controller.velocity.x = direction.normalized.x * projectileData.projSpeed;
            }
            else if (_controller.collisionState.groundBelow)
            {
                direction.y *= -1;

                _attackObject.ClearHits();
                bouncedLastFrame = true;
                _controller.velocity.x = direction.normalized.x * projectileData.projSpeed;
                _controller.velocity.y = direction.normalized.y * projectileData.projSpeed;


                if (!projectileData.projectileFlags.GetFlag(ProjectileFlagType.IgnoreGravity).GetValue())
                {
                    _controller.velocity.y = Mathf.Sqrt(_controller.velocity.y * -GambleConstants.GRAVITY);
                }

            }

            if (_controller.collisionState.groundRight || _controller.collisionState.groundLeft)
            {
                direction.x *= -1;
                _attackObject.ClearHits();
                bouncedLastFrame = true;
                _controller.velocity.y = direction.normalized.y * projectileData.projSpeed;
                _controller.velocity.x = direction.normalized.x * projectileData.projSpeed;
            }

        }
        else
        {
            bouncedLastFrame = false;
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
        movementType = data.movementType;
        spriteRenderer.sprite = data.image;
        animator.runtimeAnimatorController = data.animator;

        _attackObject.hitbox.size = data.size;
        _attackObject.attackData = data.attackData;
        _attackObject.contactFilter = data.contactFilter;
        
        foreach(ParticleSystem effect in data.visualEffects)
        {
            ParticleSystem temp = Instantiate(effect, transform);
        }

        foreach(TriggeredEffect effect in data.OnDestroyTriggers)
        {
            //TriggeredEffect temp = Instantiate(effect);
            //temp.ApplyEffect(owner);
            
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
