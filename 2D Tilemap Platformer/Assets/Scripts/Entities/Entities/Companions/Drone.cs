using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Drone : Companion
{
    public ProjectileData projectile;
    public Vector3 offset = new Vector3(1, 1, 0);
    public float fireRate = 1;
    public int damage;
    float lastFiredTimestamp = 0;
    public float smoothSpeed = .2f;

    public float followRadius = 1;

    public DroneAttack droneAttack;
    public Sightbox sightbox;

    public EnemyEntity target = null;

    protected override void Update()
    {
        FindTarget();
        Attack();
        FollowOwner();
    }

    public void FindTarget()
    {
        if (target != null)
        {
            if(!sightbox.entitiesInSight.Contains(target))
            {
                target = null;
            }
        }
        else
        {
            foreach (Entity entity in sightbox.entitiesInSight)
            {
                if (entity is EnemyEntity enemy)
                {
                    target = enemy;
                    break;
                }
            }
        }

    }

    public void FollowOwner()
    {
        float trueRadius = (companionIndex / 6)*followRadius + followRadius;
        offset = new Vector3(trueRadius * Mathf.Sin(companionIndex), trueRadius * Mathf.Cos(companionIndex));

        transform.position = Vector2.Lerp(transform.position, owner.transform.position + offset, movementSpeed*Time.deltaTime);
    }

    public void Attack()
    {

        if (projectile == null || target == null)
        {
            return;
        }


        if (Time.time > lastFiredTimestamp + (1 / fireRate))
        {
            DroneAttack attack = Instantiate(droneAttack);

            attack.SetAttacker(this);
            attack.SetDrone(this);

            StartCoroutine(attack.Activate());
        }
    }

    public void FireProjectile()
    {
        Projectile proj = Instantiate(projectile.projectileBase, transform.position, Quaternion.identity);


        if (proj != null)
        {

            Vector2 dir = Vector2.right*owner.GetDirection();

            if (target != null)
            {
                dir = (target.transform.position) - (transform.position);
            }

            proj.SetData(projectile);
            proj._attackObject.SetOwner(this);
            proj._attackObject.attackData = droneAttack.attackData;
            proj._attackObject.attackData.owner = this;
            //Need to set the projectiles data from the drones data

            if (dir == Vector2.zero)
            {
                if (!proj.projectileData.projectileFlags.GetFlag(ProjectileFlagType.IgnoreGravity).GetValue())
                {
                    dir = owner.GetDirection() * Vector2.right + Vector2.up / 2;

                }
                else
                {
                    dir = owner.GetDirection() * Vector2.right;

                }


            }


            proj.SetDirection(dir);
            lastFiredTimestamp = Time.time;

        }
    }

    public AttackData GetAttackData()
    {
        return new AttackData()
        {
            owner = this,
            damage = (int)stats.GetSecondaryStat(SecondaryStatType.DamageBonus).GetValue(),
            knockbackPower = 1,
            damageType = DamageType.Physical,
            critChance = stats.GetSecondaryStat(SecondaryStatType.CritChance).GetValue()

        };
    }

}
