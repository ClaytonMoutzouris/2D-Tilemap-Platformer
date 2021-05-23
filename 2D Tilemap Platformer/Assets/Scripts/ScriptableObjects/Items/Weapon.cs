using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Items/Weapon")]
public class Weapon : ScriptableObject
{
    public WeaponClassBase weaponBase;
    public Sprite sprite;

    public int attackIndex = 0;
    
    public WeaponAttack GetNextAttack()
    {
        WeaponAttack attack = weaponBase.attacks[attackIndex];
        attackIndex++;
        if(attackIndex >= weaponBase.attacks.Count)
        {
            attackIndex = 0;
        }

        return attack;
    }


    public virtual void LoadWeaponClass()
    {
        weaponBase = Instantiate(weaponBase);
        //weaponBase.weaponObjectPrototype.GetComponent<SpriteRenderer>().sprite = sprite;


    }
}
