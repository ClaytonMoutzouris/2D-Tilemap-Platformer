using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackData
{
    public int damage;
    public DamageType damageType;
    public float knockbackPower;
    public float knockbackAngle;
    public float critChance = 5;
    //Other goodies

    /*
    public int GetDamage()
    {
        int r = Random.Range(0, 100);
        int finalDamage = damage;
        //since the 0 is inclusive, we exclude the ceiling (crit chance)
        if (r < critChance)
        {
            finalDamage *= 2;
        }

        return finalDamage;
    }
    */
}
