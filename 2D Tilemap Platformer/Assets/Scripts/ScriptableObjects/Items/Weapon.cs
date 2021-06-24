using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Items/Weapon")]
public class Weapon : ScriptableObject
{
    public WeaponClass weaponBase;
    public Sprite sprite;
    public Projectile projectile;

    public int attackIndex = 0;
    public int damage = 5;
    public float knockbackPower;


    public Attack GetNextAttack()
    {
        Attack attack = weaponBase.attacks[attackIndex];
        attackIndex++;
        if(attackIndex >= weaponBase.attacks.Count)
        {
            attackIndex = 0;
        }

        if(attack == null)
        {
            return null;
        }
        
        return Instantiate(attack);
    }

    public Attack GetHeavyAttack()
    {
        Attack attack = weaponBase.heavyAttack;

        if (attack == null)
        {
            return null;
        }

        return Instantiate(attack);
    }

}
