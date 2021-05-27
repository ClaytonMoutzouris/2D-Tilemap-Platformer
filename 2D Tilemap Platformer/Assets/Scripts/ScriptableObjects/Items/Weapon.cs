using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Items/Weapon")]
public class Weapon : ScriptableObject
{
    public WeaponClassBase weaponBase;
    public Sprite sprite;

    public int attackIndex = 0;
    
    public Attack GetNextAttack()
    {
        Attack attack = weaponBase.attacks[attackIndex];
        attackIndex++;
        if(attackIndex >= weaponBase.attacks.Count)
        {
            attackIndex = 0;
        }

        return attack;
    }

}
