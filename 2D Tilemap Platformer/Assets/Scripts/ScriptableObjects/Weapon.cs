using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Items/Weapon")]
public class Weapon : ScriptableObject
{
    public WeaponAttack attack;
    public WeaponObject weaponObject;
}
