using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponClassType { Sword, Gun };

[CreateAssetMenu(fileName = "WeaponBase", menuName = "ScriptableObjects/Items/WeaponBase")]
public class WeaponClassBase : ScriptableObject
{
    public WeaponClassType weaponClass;
    public WeaponObject weaponObjectPrototype;
    public List<Attack> attacks;


    /*Other stuff*/
}
