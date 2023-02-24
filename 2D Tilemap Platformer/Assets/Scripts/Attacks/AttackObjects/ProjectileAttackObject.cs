using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttackObject : AttackObject
{
    public Projectile projectile;

    public override void Awake()
    {
        base.Awake();

        projectile = GetComponent<Projectile>();

    }

    public override AttackHitData HitEnemy(IHurtable hit)
    {

        attackData = GetAttackData();

        AttackHitData hitData = base.HitEnemy(hit);

        Weapon weapon = projectile.weapon;

        if(weapon)
        {
            foreach (Effect effect in weapon.OnHitGainEffects)
            {
                Effect temp = Instantiate(effect);
                temp.ApplyEffect(owner, owner, hitData);
            }

            foreach (Effect effect in weapon.OnHitInflictEffects)
            {
                Effect temp = Instantiate(effect);
                temp.ApplyEffect(owner, hitData.hit.GetEntity(), hitData);
            }

            if (hitData.killedEnemy)
            {
                foreach (Effect effect in weapon.OnKillEffects)
                {
                    effect.ApplyEffect(owner, owner, hitData);
                }
            }

            weapon.ComboUp();
        }

        return hitData;
    }

}
