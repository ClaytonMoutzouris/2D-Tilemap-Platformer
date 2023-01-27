using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackData
{
    [HideInInspector]
    public Entity owner;
    public int damage;
    public DamageType damageType;
    public float knockbackPower;
    public float critChance = 5;

    [HideInInspector]
    public bool crit = false;
    //Other goodies

    
    public int GetDamage()
    {
        float finalDamage = damage;

        if (owner != null)
        {
            finalDamage += (int)owner.stats.GetSecondaryStat(SecondaryStatType.DamageBonus).GetValue();

            int r = Random.Range(0, 100);

            crit = false;

            //since the 0 is inclusive, we exclude the ceiling (crit chance)
            if (r < critChance)
            {
                crit = true;
                finalDamage *= owner.stats.GetSecondaryStat(SecondaryStatType.CritDamage).GetValue() / 100.0f;

            }
        }

        return (int)finalDamage;
    }

    public AttackHitData GetHitData(IHurtable hit)
    {
        AttackHitData hitData = new AttackHitData(hit);
        hitData.attackOwner = owner;
        hitData.knockbackPower = knockbackPower;
        hitData.damageDealt = GetDamage();
        hitData.crit = crit;
        if(crit)
        {
            hitData.hitResult = AttackHitResult.Crit;
        }
        return hitData;
    }
    
}

public enum AttackHitResult { Hit, Crit, Miss, Dodge, Block };

public class AttackHitData
{
    public Entity attackOwner;
    public IHurtable hit;
    public AttackHitResult hitResult = AttackHitResult.Hit;
    public int damageDealt;
    public bool crit;
    public DamageType damageType;
    public float knockbackPower;
    public float knockbackAngle;

    public AttackHitData(IHurtable hit)
    {
        this.hit = hit;
    }

}