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
    //Might want to create a seperate projectile class for "boomerangs", but modularity has its merits aswell
    bool returning = false;

    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Vector3 _velocity = Vector3.zero;
    public bool bouncedLastFrame = false;


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
            _velocity.y = Mathf.Sqrt(projectileData.projSpeed *direction.normalized.y * -GambleConstants.GRAVITY);
            //Debug.Log("Velocity " + _velocity);

        }
        else
        {
            _velocity.x = direction.normalized.x * projectileData.projSpeed;
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

        _velocity.x = direction.normalized.x * projectileData.projSpeed;


        if (projectileData.projectileFlags.GetFlag(ProjectileFlagType.IgnoreGravity).GetValue())
        {
            _velocity.y = direction.normalized.y * projectileData.projSpeed;
        }  else
        {
            _velocity.y += GambleConstants.GRAVITY * Time.deltaTime;
            //Debug.Log("Velocity " + _velocity);


        }


        if (projectileData.projectileFlags.GetFlag(ProjectileFlagType.Boomerang).GetValue())
        {
            Boomerang(ref _velocity);
        }
        
        if(projectileData.projectileFlags.GetFlag(ProjectileFlagType.IsAngled).GetValue())
        {
            Vector2 dir = _velocity.normalized;
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, -angle);
        }

        _controller.move(_velocity * Time.deltaTime);

        _velocity = _controller.velocity;

        if (_controller.collisionState.hasCollision())
        {
            if (projectileData.projectileFlags.GetFlag(ProjectileFlagType.DestroyOnGround).GetValue())
            {
                Debug.Log("Destroy on ground");
                DestroyProjectile();
            } else if(projectileData.projectileFlags.GetFlag(ProjectileFlagType.Bounce).GetValue() && !bouncedLastFrame)
            {
                if(_controller.collisionState.above)
                {
                    direction.y *= -1;
                } else if (_controller.collisionState.below)
                {
                    direction.y *= -1;
                    if (!projectileData.projectileFlags.GetFlag(ProjectileFlagType.IgnoreGravity).GetValue())
                    {
                        _velocity.y = Mathf.Sqrt(projectileData.projSpeed * direction.y * -GambleConstants.GRAVITY);
                    }
                }

                if (_controller.collisionState.left || _controller.collisionState.right)
                {
                    direction.x *= -1;

                }
                _attackObject.ClearHits();
                bouncedLastFrame = true;
            }
        } else
        {
            bouncedLastFrame = false;

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
        projectileData.projectileFlags.AddBonuses(wep.projectileBonuses);
        projectileData.image = wep.sprite;
        spriteRenderer.color = wep.color;
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

        if(projectileData.projectileFlags.GetFlag(ProjectileFlagType.IgnoreGround).GetValue())
        {
            _controller.platformMask = new LayerMask();
        }
    }
}
