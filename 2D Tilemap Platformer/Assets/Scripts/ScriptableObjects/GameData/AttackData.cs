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

    
    public int GetDamage()
    {
        int r = Random.Range(0, 100);
        int finalDamage = damage;

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

        return finalDamage;
    }
    
}
