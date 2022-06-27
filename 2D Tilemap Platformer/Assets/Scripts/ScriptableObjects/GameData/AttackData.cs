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
    public float knockbackAngle;
    public float critChance = 5;

    [HideInInspector]
    public bool crit = false;
    //Other goodies

    
    public int GetDamage(Entity hit)
    {
        int r = Random.Range(0, 100);
        int finalDamage = damage;
        int damageReduction = 0;

        if (owner != null)
        {
            finalDamage += (int)owner.stats.GetSecondaryStat(SecondaryStatType.DamageBonus).GetValue();
        }

        crit = false;

        //since the 0 is inclusive, we exclude the ceiling (crit chance)
        if (r < critChance)
        {
            crit = true;
            finalDamage *= 2;

        }

        //Before or after crit?
        if (hit != null)
        {
            //This one reduced by 1% per damage reduction
            //damageReduction = Mathf.FloorToInt((int)(finalDamage * ((hit.stats.GetSecondaryStat(SecondaryStatType.DamageReduction).GetValue()) / 100)));

            //This reduces damage by 1 for every 5 Damage Reduction (which is 1 to 1 with defense atm)
            damageReduction = Mathf.FloorToInt(hit.stats.GetSecondaryStat(SecondaryStatType.DamageReduction).GetValue() / 5);
            finalDamage -= damageReduction;

        }

        return finalDamage;
    }
    
}


public class AttackHitData
{
    public Entity attackOwner;
    public Entity hitEntity;

    public int damageDealt;
    public bool crit;
}