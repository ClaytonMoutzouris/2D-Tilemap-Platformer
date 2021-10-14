using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsBody2D))]
[RequireComponent(typeof(AttackObject))]
public class ExplosiveProjectile : Projectile
{
    //This doesnt have to be an explosion, could be like little missiles or something
    public ProjectileData explosion;
    int damage;
    float knockbackPower;

    public override void DestroyProjectile()
    {
        Projectile projectile = Instantiate(explosion.projectileBase, transform.position, Quaternion.identity);
        projectile.SetData(explosion);
        projectile._attackObject.SetOwner(_attackObject.owner);

        projectile._attackObject.attackData.damage = damage;
        projectile._attackObject.attackData.knockbackPower = knockbackPower;

        base.DestroyProjectile();
    }

    public override void SetFromWeapon(Weapon wep)
    {
        _attackObject.SetOwner(wep.owner);

        _attackObject.attackData = wep.GetAttackData();
        damage = wep.GetAttackData().damage;
        knockbackPower = wep.GetAttackData().knockbackPower;

        _attackObject.attackData.damage = 0;
        _attackObject.attackData.knockbackPower = 0;

        projectileData.projSpeed = wep.GetStatValue(WeaponAttributesType.ProjectileSpeed);
        projectileData.projectileFlags.AddBonuses(wep.projectileBonuses);



    }

}
