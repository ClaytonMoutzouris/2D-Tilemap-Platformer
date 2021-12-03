using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponClassType { Sword, Gun, Axe, Dagger, Thrown };

[CreateAssetMenu(fileName = "WeaponClass", menuName = "ScriptableObjects/Items/WeaponClass")]
public class WeaponClass : ScriptableObject
{
    public WeaponClassType weaponClass;
    public WeaponObject weaponObjectPrototype;
    public List<Attack> attacks;
    public Attack heavyAttack;
    /*Other stuff*/
}
