using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType { Physical, Magical }
public enum AttackInput { Up, Down, Forward, Backward, Neutral, Shoot, AirDown }
public enum WeaponSlot { Melee, Ranged }

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Items/Equipment/Weapon")]
public class Weapon : Equipment
{
    [Header("Weapon Class")]
    public WeaponClassType weaponClass;
    public WeaponSlot weaponSlot;

    public WeaponObject weaponBase;

    [Header("Attacks")]
    public List<WeaponAttack> attacks;
    public WeaponAttack rangedAttack;
    public WeaponAttack heavyAttack;

    public WeaponAttack attackUp;
    public WeaponAttack attackDown;
    public WeaponAttack attackForward;
    public WeaponAttack attackBack;
    public WeaponAttack attackNeutral;
    public WeaponAttack airDownAttack;

    public ProjectileData baseProjectile;

    public AmmoType ammoType;
    public int currentAmmo = 5;


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

    [Header("Effects")]
    public List<Effect> OnHitGainEffects;
    public List<Effect> OnHitInflictEffects;

    //This is for projectiles only?
    public List<Effect> OnDestroyEffects;
    public List<Effect> OnAttackGainEffects;
    public List<Effect> OnAttackInflictEffects;
    public List<Effect> OnKillEffects;


    int attackIndex = 0;
    float lastFiredTimestamp = 0;
    private WeaponObject weaponObj;

    float lastComboTimestamp = 0;
    int combo = 0;
    public float comboDuration = 3f;

    public void ComboUp()
    {
        if(Time.time > lastComboTimestamp + comboDuration)
        {
            combo = 0;
        }

        combo++;
        lastComboTimestamp = Time.time;
    }

    public void CheckCombo()
    {
        if (Time.time > lastComboTimestamp + comboDuration)
        {
            combo = 0;
        }
    }

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
        currentAmmo = (int)weaponAttributes.GetAttribute(WeaponAttributesType.AmmoCapacity).GetValue();
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

        int ammoWhenEquipped = currentAmmo;
        int capacityWhenEquipped = (int)weaponAttributes.GetAttribute(WeaponAttributesType.AmmoCapacity).GetValue();


        foreach (Ability ability in owner.abilities)
        {
            ability.OnEquippedWeapon(this);
        }
        

        foreach (Talent talent in owner.learnedTalents)
        {
            weaponAttributes.AddBonuses(talent.weaponBonuses);
        }



        base.OnEquipped(entity);

        if (capacityWhenEquipped != (int)weaponAttributes.GetAttribute(WeaponAttributesType.AmmoCapacity).GetValue())
        {
            currentAmmo = Mathf.Max(ammoWhenEquipped, currentAmmo + ((int)weaponAttributes.GetAttribute(WeaponAttributesType.AmmoCapacity).GetValue() - capacityWhenEquipped));
        }

        //Set anything that needs to be set based on our starting stats here

        owner._attackManager.SetWeaponObject(this);

    }

    public override void OnUnequipped(CharacterEntity entity)
    {
        int ammoWhenEquipped = currentAmmo;
        int capacityWhenEquipped = (int)weaponAttributes.GetAttribute(WeaponAttributesType.AmmoCapacity).GetValue();

        foreach (Ability ability in entity.abilities)
        {
            ability.OnUnequippedWeapon(this);

        }
        

        foreach (Talent talent in owner.learnedTalents)
        {
            weaponAttributes.RemoveBonuses(talent.weaponBonuses);
        }

        base.OnUnequipped(entity);

        if (capacityWhenEquipped != (int)weaponAttributes.GetAttribute(WeaponAttributesType.AmmoCapacity).GetValue())
        {
            currentAmmo = Mathf.Max(ammoWhenEquipped, currentAmmo + ((int)weaponAttributes.GetAttribute(WeaponAttributesType.AmmoCapacity).GetValue() - capacityWhenEquipped));
        }
        //Set anything that needs to be set based on our startins stats here

    }

    public WeaponAttack GetAttack(AttackInput attackDir)
    {
        WeaponAttack wepAttack = attackNeutral;

        switch (attackDir)
        {
            case AttackInput.Down:
                wepAttack = attackDown;
                break;
            case AttackInput.Up:
                wepAttack = attackUp;
                break;
            case AttackInput.Forward:
                wepAttack = attackForward;
                break;
            case AttackInput.Backward:
                wepAttack = attackBack;
                break;
            case AttackInput.Neutral:
                wepAttack = attackNeutral;
                break;
            case AttackInput.Shoot:
                wepAttack = rangedAttack;
                break;
            case AttackInput.AirDown:
                wepAttack = airDownAttack;
                break;
        }

        return Instantiate(wepAttack);

    }

    public WeaponAttack GetHeavyAttack()
    {
        if (heavyAttack == null)
        {
            return null;
        }


        return Instantiate(heavyAttack);
    }

    public WeaponAttack GetRangedAttack()
    {
        if (rangedAttack == null)
        {
            return null;
        }


        return Instantiate(rangedAttack);
    }

    public void FireProjectile(int angle = 0)
    {
        if(baseProjectile == null)
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
            if (currentAmmo <= 0)
            {
                owner.ShowFloatingText("Reload", Color.red);
                return;
            }


            lastFiredTimestamp = Time.time;
            currentAmmo--;
            owner.playerVersusUI.ammoDisplay.UpdateAmmo();

            Projectile proj = Instantiate(baseProjectile.projectileBase, owner.transform.position, Quaternion.identity);

            if (proj != null)
            {
                proj.SetData(baseProjectile);
                proj.SetFromWeapon(this);

                dir.x *= owner.GetDirection();

                proj.SetDirection(dir);
            }
        }

    }

    public void FireAimedProjectile()
    {

        if (baseProjectile == null)
        {
            return;
        }

        Vector2 dir = owner._input.GetRightStickAim().normalized;

        //If there is no input, default the direction to forward

        if (Time.time > lastFiredTimestamp + (1 / GetStatValue(WeaponAttributesType.FireRate)))
        {

            if (currentAmmo <= 0)
            {
                owner.ShowFloatingText("Reload", Color.red);
                lastFiredTimestamp = Time.time;

                return;
            }

            currentAmmo--;
            owner.playerVersusUI.ammoDisplay.UpdateAmmo();

            lastFiredTimestamp = Time.time;

            Projectile proj = Instantiate(baseProjectile.projectileBase, owner.transform.position, Quaternion.identity);


            if (proj != null)
            {
                proj.SetData(baseProjectile);
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

    public void ConsumeAmmo(int amount = 1)
    {
        currentAmmo -= amount;
        owner.playerVersusUI.ammoDisplay.UpdateAmmo();
    }

    public Projectile FireLaserProjectile()
    {

        if (baseProjectile == null)
        {
            return null;
        }

        Vector2 dir = owner._input.GetRightStickAim().normalized;

        //If there is no input, default the direction to forward

        if (Time.time > lastFiredTimestamp + (1 / GetStatValue(WeaponAttributesType.FireRate)))
        {

            if (currentAmmo <= 0)
            {
                owner.ShowFloatingText("Reload", Color.red);
                lastFiredTimestamp = Time.time;

                return null;
            }

            lastFiredTimestamp = Time.time;

            Projectile proj = Instantiate(baseProjectile.projectileBase, owner.transform.position, Quaternion.identity);


            if (proj != null)
            {
                proj.SetData(baseProjectile);
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

                return proj;

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

        return null;
    }

    //Only works for melee weapons with the default thrown object set
    public void ThrowWeapon()
    {
        Projectile proj = Instantiate(baseProjectile.projectileBase, owner.transform.position, Quaternion.identity);

        if (proj != null)
        {

            proj.SetData(baseProjectile);
            proj.SetFromWeapon(this);
            if(weaponSlot == WeaponSlot.Melee)
            {
                proj._attackObject.UpdateHitbox(weaponObj.hitbox.size, weaponObj.hitbox.offset);
            }

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
            critChance = (GetStatValue(WeaponAttributesType.CritChance) + owner.stats.GetSecondaryStat(SecondaryStatType.CritChance).GetValue())

        };


    }

    public void UpdateAmmoCapacity()
    {
        int ammoWhenEquipped = currentAmmo;
        int capacityWhenEquipped = (int)weaponAttributes.GetAttribute(WeaponAttributesType.AmmoCapacity).GetValue();



        if (capacityWhenEquipped != (int)weaponAttributes.GetAttribute(WeaponAttributesType.AmmoCapacity).GetValue())
        {
            currentAmmo = Mathf.Max(ammoWhenEquipped, currentAmmo + ((int)weaponAttributes.GetAttribute(WeaponAttributesType.AmmoCapacity).GetValue() - capacityWhenEquipped));
            owner.playerVersusUI.ammoDisplay.UpdateAmmo();

        }
    }

    public void Reload(AmmoType ammoType)
    {
        if(ammoType == this.ammoType || ammoType == AmmoType.None)
        {
            currentAmmo = (int)weaponAttributes.GetAttribute(WeaponAttributesType.AmmoCapacity).GetValue();
            owner.playerVersusUI.ammoDisplay.UpdateAmmo();

        }
    }
}
