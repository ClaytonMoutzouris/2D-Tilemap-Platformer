using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ammunition", menuName = "ScriptableObjects/Items/Ammunition")]
public class Ammunition : ItemData
{
    [Header("Ammunition Info")]
    public AmmoType ammoType;

    //possible others - amount, quality, etc
}
