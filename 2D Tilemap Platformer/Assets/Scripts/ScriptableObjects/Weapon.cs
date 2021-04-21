using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Items/Weapon")]
public class Weapon : ScriptableObject
{
    public WeaponAttack attack;
    public List<WeaponAttack> attacks;
    public WeaponObject weaponObject;

    int attackIndex = 0;
    
    public WeaponAttack GetNextAttack()
    {
        WeaponAttack attack = attacks[attackIndex];
        attackIndex++;
        if(attackIndex >= attacks.Count)
        {
            attackIndex = 0;
        }

        return attack;
    }

}
