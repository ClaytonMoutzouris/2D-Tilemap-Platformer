using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponClassType { Sword, Gun, Axe, Dagger, Thrown };

[CreateAssetMenu(fileName = "WeaponClass", menuName = "ScriptableObjects/Items/WeaponClass")]
public class WeaponClass : ScriptableObject
{
    public WeaponClassType weaponClass;
    public WeaponObject weaponObjectPrototype;
    public List<WeaponAttack> attacks;
    public WeaponAttack heavyAttack;
    /*Other stuff*/
}
