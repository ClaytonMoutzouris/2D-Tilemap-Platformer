using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType { Physical, Magical }
public enum AttackInput { Up, Down, Forward, Backward, Neutral, Shoot }
[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Items/Equipment/Weapon")]
public class Weapon : Equipment
{
    [Header("Weapon Class")]
    public WeaponClassType weaponClass;
    public WeaponObject weaponBase;

    [Header("Attacks")]
    public List<Attack> attacks;
    public Attack heavyAttack;

    public Attack attackUp;
    public Attack attackDown;
    public Attack attackForward;
    public Attack attackBack;
    public Attack attackNeutral;

    public ProjectileData projectile;



    [Header("Stats")]
    [HideInInspector]
    public List<ProjectileFlagBonus> projectileBonuses = new List<ProjectileFlagBonus>();
    //This is the actual object
    [HideInInspector]
    public WeaponAttributes weaponAttributes;
    [HideInInspector]
    //public List<ProjectileFlagBonus> projectileEffects = new List<ProjectileFlagBonus>();
    private bool baseStatsLoaded = false;


    [Header("Base Stats")]
    public int damage;
    public DamageType damageType;
    public float knockbackAngle;
    public float knockbackPower;
    public float critChance;
    public float attackSpeed;
    public float weaponReach;
    public float fireRate;
    public float projectileSpeed;
    public float projectileLifeTime = 2;
    public float numberOfProjectiles;
    public float spreadAngle;
    public int ammoCapacity;
    public float reloadTime;

    int attackIndex = 0;
    float lastFiredTimestamp = 0;
    private WeaponObject weaponObj;

    //This is basically an INIT method
    public void SetBaseStats()
    {
        weaponAttributes = new WeaponAttributes();

        weaponAttributes.SetAttribute(new WeaponAttribute(WeaponAttributesType.Damage, damage));
        weaponAttributes.SetAttribute(new WeaponAttribute(WeaponAttributesType.DamageType, (int)damageType));
        weaponAttributes.SetAttribute(new WeaponAttribute(WeaponAttributesType.KnockbackAngle, knockbackAngle));
        weaponAttributes.SetAttribute(new WeaponAttribute(WeaponAttributesType.KnockbackPower, knockbackPower));
        weaponAttributes.SetAttribute(new WeaponAttribute(WeaponAttributesType.CritChance, critChance));
        weaponAttributes.SetAttribute(new WeaponAttribute(WeaponAttributesType.AttackSpeed, attackSpeed));
        weaponAttributes.SetAttribute(new WeaponAttribute(WeaponAttributesType.WeaponReach, weaponReach));
        weaponAttributes.SetAttribute(new WeaponAttribute(WeaponAttributesType.FireRate, fireRate));
        weaponAttributes.SetAttribute(new WeaponAttribute(WeaponAttributesType.ProjectileSpeed, projectileSpeed));
        weaponAttributes.SetAttribute(new WeaponAttribute(WeaponAttributesType.NumProjectiles, numberOfProjectiles));
        weaponAttributes.SetAttribute(new WeaponAttribute(WeaponAttributesType.SpreadAngle, spreadAngle));
        weaponAttributes.SetAttribute(new WeaponAttribute(WeaponAttributesType.AmmoCapacity, ammoCapacity));
        weaponAttributes.SetAttribute(new WeaponAttribute(WeaponAttributesType.ReloadTime, reloadTime));
        weaponAttributes.SetAttribute(new WeaponAttribute(WeaponAttributesType.ProjectileLifeTime, projectileLifeTime));

        baseStatsLoaded = true;
    }

    public void SetObject(WeaponObject obj)
    {
        weaponObj = obj;
    }

    //Helper method for easier stat gets (if we only need the value)
    public float GetStatValue(WeaponAttributesType type)
    {
        return weaponAttributes.GetAttribute(type).GetValue();
    }

    //Helper method for easier stat gets 
    public WeaponAttribute GetStat(WeaponAttributesType type)
    {
        return weaponAttributes.GetAttribute(type);
    }

    public override void OnEquipped(PlayerController entity)
    {
        owner = entity;

        //Simple fix, probably never need to change it though
        if (!baseStatsLoaded)
        {
            SetBaseStats();
        }

        
        foreach (Ability ability in owner.abilities)
        {
            ability.OnEquippedWeapon();
        }
        

        foreach (Talent talent in owner.learnedTalents)
        {
            weaponAttributes.AddBonuses(talent.weaponBonuses);
        }

        base.OnEquipped(entity);


        owner._attackManager.SetWeaponObject(this);

    }

    public override void OnUnequipped(Entity entity)
    {
        
        foreach (Ability ability in entity.abilities)
        {
            ability.OnUnequippedWeapon();

        }
        

        foreach (Talent talent in owner.learnedTalents)
        {
            weaponAttributes.RemoveBonuses(talent.weaponBonuses);
        }

        base.OnUnequipped(entity);

    }


    public Attack GetNextAttack()
    {
        if (attacks[attackIndex] == null)
        {
            return null;
        }

        Attack attack = Instantiate(attacks[attackIndex]);
        attackIndex++;
        if(attackIndex >= attacks.Count)
        {
            attackIndex = 0;
        }

        attack.attackSpeed = GetStatValue(WeaponAttributesType.AttackSpeed);

        //owner.StartCoroutine(attack.Activate(owner));


        
        return attack;
    }

    public Attack GetAttack(AttackInput attackDir)
    {


        switch(attackDir)
        {
            case AttackInput.Down:
                return attackDown;
            case AttackInput.Up:
                return attackUp;
            case AttackInput.Forward:
                return attackForward;
            case AttackInput.Backward:
                return attackBack;
            case AttackInput.Neutral:
                return attackNeutral;
            case AttackInput.Shoot:
                return attacks[0];
        }

        return attacks[0];

    }

    public Attack GetHeavyAttack()
    {
        if (heavyAttack == null)
        {
            return null;
        }

        Attack attack = Instantiate(heavyAttack);



        return attack;
    }

    public void FireProjectile(int angle = 0)
    {
        if(projectile == null)
        {
            return;
        }

        Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, angle) * Vector2.right);

        if (weaponObj != null)
        {
            owner._attackManager.rangedWeaponObject.AimWeapon(dir);
        }


        if (Time.time > lastFiredTimestamp + (1 / GetStatValue(WeaponAttributesType.FireRate)))
        {

            lastFiredTimestamp = Time.time;

            Projectile proj = Instantiate(projectile.projectileBase, owner.transform.position, Quaternion.identity);

            if (proj != null)
            {
                proj.SetData(projectile);
                proj.SetFromWeapon(this);

                dir.x *= owner.GetDirection();

                proj.SetDirection(dir);
            }
        }






    }

    public void FireAimedProjectile()
    {

        if (projectile == null)
        {
            return;
        }

        Vector2 dir = owner._input.GetRightStickAim().normalized;

        //If there is no input, default the direction to forward

        if (Time.time > lastFiredTimestamp + (1 / GetStatValue(WeaponAttributesType.FireRate)))
        {
            lastFiredTimestamp = Time.time;

            Projectile proj = Instantiate(projectile.projectileBase, owner.transform.position, Quaternion.identity);


            if (proj != null)
            {
                proj.SetData(projectile);
                proj.SetFromWeapon(this);

                if (dir == Vector2.zero)
                {
                    if (!proj.projectileData.projectileFlags.GetFlag(ProjectileFlagType.IgnoreGravity).GetValue())
                    {
                        dir = owner.GetDirection() * Vector2.right + Vector2.up / 2;

                    } else
                    {
                        dir = owner.GetDirection() * Vector2.right;

                    }


                }

                
                proj.SetDirection(dir);
            }
        }


        if (weaponObj != null)
        {
            if (dir == Vector2.zero)
            {

                dir = owner.GetDirection() * Vector2.right;

            }
            owner._attackManager.rangedWeaponObject.AimWeapon(dir);
        }
    }

    //Only works for melee weapons with the default thrown object set
    public void ThrowWeapon()
    {
        Projectile proj = Instantiate(projectile.projectileBase, owner.transform.position, Quaternion.identity);

        if (proj != null)
        {
            proj.SetData(projectile);
            proj.SetFromWeapon(this);
            proj._attackObject.UpdateHitbox(weaponBase.hitbox.size, weaponBase.hitbox.offset);
            proj.spriteRenderer.sprite = sprite;
            Vector2 dir;

            //If there is no input, default the direction to forward

            if(proj.projectileData.projectileFlags.GetFlag(ProjectileFlagType.IgnoreGravity).GetValue())
            {
                dir = owner.GetDirection() * Vector2.right;

            } else
            {
                dir = owner.GetDirection() * Vector2.right + Vector2.up/2;

            }
            

            proj.SetDirection(dir);
        }
    }

    public AttackData GetAttackData()
    {
        return new AttackData()
        {
            owner = this.owner,
            damage = (int)GetStatValue(WeaponAttributesType.Damage),
            knockbackPower = GetStatValue(WeaponAttributesType.KnockbackPower),
            damageType = (DamageType)GetStatValue(WeaponAttributesType.DamageType),
            knockbackAngle = GetStatValue(WeaponAttributesType.KnockbackAngle),
            critChance = (GetStatValue(WeaponAttributesType.CritChance) + owner.stats.GetSecondaryStat(SecondaryStatType.CritChance).GetValue())

        };


    }
}
