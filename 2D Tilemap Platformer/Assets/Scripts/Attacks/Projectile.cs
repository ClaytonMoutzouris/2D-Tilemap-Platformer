using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileMovementType { Straight, Arc, Returning, Control, Homing, Laser, Static, Boomerang }

[RequireComponent(typeof(PhysicsBody2D))]
[RequireComponent(typeof(AttackObject))]
public class Projectile : Entity
{
    [Header("Projectile Properties")]
    public ProjectileData projectileData;

    public Vector2 direction;
    protected float startTime = 0;

    public AttackObject _attackObject;

    //Physics things
    public LayerMask baseLayermask;
    public LayerMask laserLayerMask;

    //Might want to create a seperate projectile class for "boomerangs", but modularity has its merits aswell
    bool returning = false;
    
    public bool bouncedLastFrame = false;
    public ProjectileMovementType movementType;
    public List<Effect> onDestroyedEffects;
    public Vector3 boomerangCenter;
    public Weapon weapon;
    public Entity target;
    public Sightbox sightbox;
    public int turnSpeed = 3;
    public Chain activeChain;

    public Vector3 laserOrigin;
    public float laserSpeed = 0.5f;
    public float laserLength = 100;
    float laserRefreshTimeStamp = 0;
    

    protected override void Awake()
    {
        base.Awake();
        _attackObject = GetComponent<AttackObject>();
        baseLayermask = _controller.platformMask;
        // Might need this later?
        /*
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
        */

        if (sightbox && movementType == ProjectileMovementType.Homing)
        {
            sightbox.state = ColliderState.Open;
        }
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

        if(movementType == ProjectileMovementType.Boomerang)
        {
            if(direction.x < 0)
            {
                spriteRenderer.flipX = true;
            }

            boomerangCenter = transform.position + (Vector3)direction.normalized * projectileData.boomerangRadius;

            _controller.velocity = Vector2.Perpendicular(boomerangCenter.normalized)*projectileData.projSpeed;
        }


        if(projectileData.chainPrefab)
        {
            activeChain = Instantiate(projectileData.chainPrefab);
            activeChain.SetObjects(_attackObject.owner.gameObject, gameObject);
        }

        laserOrigin = _controller.transform.position;
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
            case (ProjectileMovementType.Returning):
                Returning();
                break;
            case (ProjectileMovementType.Control):
                ControlMovement();
                break;
            case (ProjectileMovementType.Homing):
                HomingMovement();
                break;
            case (ProjectileMovementType.Laser):
                Laser();
                break;
            case (ProjectileMovementType.Static):
                _controller.velocity = Vector3.zero;
                break;
            case (ProjectileMovementType.Boomerang):
                Boomerang();
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

        if (startTime + projectileData.lifeTime <= Time.time && movementType != ProjectileMovementType.Returning && movementType != ProjectileMovementType.Laser)
        {
            DestroyProjectile();
        }

        //Destroy the object when it makes a collision unless it has piercing
        if (!projectileData.projectileFlags.GetFlag(ProjectileFlagType.Piercing).GetValue() && _attackObject.hits.Count > 0)
        {
            DestroyProjectile();
        }
    }

    public void Laser()
    {
        _controller.velocity = Vector3.zero;

        laserOrigin = _attackObject.owner.transform.position;

        if(_attackObject.owner is PlayerController player)
        {
            Vector2 newDir = player._input.GetRightStickAim().normalized;
            if(newDir != Vector2.zero)
            {
                direction = newDir;
            }
        }

        RaycastHit2D[] hits = Physics2D.RaycastAll(laserOrigin, direction, laserLength, _attackObject.contactFilter.layerMask + laserLayerMask);
        float distance = laserLength;
        Vector3 dest = direction.normalized * laserLength;

        foreach (RaycastHit2D hit in hits)
        {
            Entity entity = hit.collider.GetComponent<Entity>();
            if (entity == this || entity == _attackObject.owner)
            {
                continue;
            }

            Entity parEntity = hit.collider.GetComponentInParent<Entity>();
            if (parEntity == this || parEntity == _attackObject.owner)
            {
                continue;
            }
            dest = hit.point;


            //_attackObject.hitbox.size = spriteRenderer.bounds.size;

            break;
        }

            distance = Vector3.Distance(laserOrigin, dest);

            spriteRenderer.drawMode = SpriteDrawMode.Tiled;
            spriteRenderer.tileMode = SpriteTileMode.Continuous;
            transform.localScale = Vector3.one;
            spriteRenderer.size = new Vector2(.25f, distance);

            Vector3 middlePoint = (laserOrigin + dest) / 2;
            transform.position = middlePoint;

            Vector3 rotationDirection = (dest - laserOrigin);
            transform.up = rotationDirection;

            _attackObject.hitbox.autoTiling = true;

        if(Time.time > laserRefreshTimeStamp + laserSpeed)
        {
            laserRefreshTimeStamp = Time.time;
            _attackObject.ClearHits();

            if(weapon)
            {
                weapon.ConsumeAmmo();
            }
        }

    }

    public void StraightMovement()
    {


    }

    public void ArcMovement()
    {


    }

    public void HomingMovement()
    {

        if (target != null)
        {
            direction = (target.transform.position - transform.position).normalized;


            _controller.velocity.y += direction.y * projectileData.projSpeed * turnSpeed * Time.deltaTime;
            _controller.velocity.x += direction.x * projectileData.projSpeed * turnSpeed * Time.deltaTime;

            _controller.velocity = Vector2.ClampMagnitude(_controller.velocity, projectileData.projSpeed);
        } else
        {
            FindTarget();

        }

    }

    public void FindTarget()
    {
        if (target != null)
        {
            if (!sightbox.entitiesInSight.Contains(target))
            {
                target = null;
            }
        }
        else
        {
            foreach (Entity entity in sightbox.entitiesInSight)
            {
                if (entity != this && entity != _attackObject.owner)
                {
                    target = entity;
                    break;
                }
            }
        }

    }

    public void ControlMovement()
    {
        if (_attackObject.owner is PlayerController player)
        {
            Vector2 aim = player._input.GetRightStickAim().normalized;

            if (aim != Vector2.zero)
            {
                _controller.velocity.x += (aim.x * projectileData.projSpeed) * Time.deltaTime;
                _controller.velocity.y += (aim.y * projectileData.projSpeed) * Time.deltaTime;
            }
            else
            {

            }
        }


    }


    public virtual void DestroyProjectile()
    {
        foreach(Effect effect in onDestroyedEffects)
        {
            Effect temp = Instantiate(effect);
            temp.ApplyEffect(this);
        }

        if(activeChain)
        {
            Destroy(activeChain.gameObject);
        }

        Destroy(gameObject);
    }

    public void Boomerang()
    {

        float timeElapsed = Time.time - startTime;

        float forceMag = Mathf.Pow(projectileData.projSpeed, 2.0f) * projectileData.boomerangRadius*0.5f;
        Vector2 forceVec = (boomerangCenter - transform.position).normalized;

        _controller.velocity.x += forceVec.x * forceMag*Time.deltaTime;
        _controller.velocity.y += forceVec.y * forceMag*Time.deltaTime;

    }

    public void Returning()
    {
        Vector2 aim = _attackObject.owner.transform.position - transform.position;

        if (!returning && Time.time - startTime > projectileData.lifeTime / 2)
        {
            returning = true;
            _controller.velocity.y = aim.normalized.y * projectileData.projSpeed;
            _controller.velocity.x = aim.normalized.x * projectileData.projSpeed;
            _attackObject.ClearHits();

        }

        if (returning)
        {
            if (aim != Vector2.zero)
            {
                _controller.velocity.y = aim.normalized.y * projectileData.projSpeed;
                _controller.velocity.x = aim.normalized.x * projectileData.projSpeed;
            }

            if(aim.magnitude < .1f)
            {
                DestroyProjectile();
            }
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
        weapon = wep;
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

        foreach(Effect effect in wep.OnDestroyEffects)
        {
            onDestroyedEffects.Add(Instantiate(effect));
        }


        _controller.ignoreGravity = projectileData.projectileFlags.GetFlag(ProjectileFlagType.IgnoreGravity).GetValue();



    }

    public void SetData(ProjectileData data)
    {
        projectileData = Instantiate(data);
        projectileData.SetBaseFlags();
        movementType = data.movementType;
        spriteRenderer.sprite = data.image;
        _animator.runtimeAnimatorController = data.animator;

        _attackObject.hitbox.size = data.size;
        _attackObject.attackData = data.attackData;
        _attackObject.contactFilter = data.contactFilter;
        
        foreach(ParticleSystem effect in data.visualEffects)
        {
            ParticleSystem temp = Instantiate(effect, transform);
        }

        foreach(Effect effect in data.OnDestroyTriggers)
        {
            Effect temp = Instantiate(effect);
            onDestroyedEffects.Add(temp);
        }

        if (projectileData.projectileFlags.GetFlag(ProjectileFlagType.IgnoreGround).GetValue())
        {
            _controller.platformMask = new LayerMask();
        } else
        {
            _controller.platformMask = baseLayermask;
        }

        _controller.ignoreGravity = projectileData.projectileFlags.GetFlag(ProjectileFlagType.IgnoreGravity).GetValue();


        if (sightbox && movementType == ProjectileMovementType.Homing)
        {
            sightbox.state = ColliderState.Open;
        }
    }
}
