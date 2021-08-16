using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsBody2D))]
[RequireComponent(typeof(AttackObject))]
public class ExplosiveProjectile : Projectile
{
    public Projectile explosion;
    int damage;
    float knockbackPower;

    public void OnDestroy()
    {
        Projectile projectile = Instantiate(explosion, transform.position, Quaternion.identity);
        projectile._attackObject.SetOwner(_attackObject.owner);

        projectile._attackObject.attackData.damage = damage;
        projectile._attackObject.attackData.knockbackPower = knockbackPower;

    }

    public override void SetFromWeapon(Weapon wep)
    {
        _attackObject.SetOwner(wep.owner);

        _attackObject.attackData = wep.GetAttackData();
        damage = wep.GetAttackData().damage;
        knockbackPower = wep.GetAttackData().knockbackPower;

        _attackObject.attackData.damage = 0;
        _attackObject.attackData.knockbackPower = 0;

        projSpeed = wep.GetStatValue(WeaponAttributesType.ProjectileSpeed);

    }

}
