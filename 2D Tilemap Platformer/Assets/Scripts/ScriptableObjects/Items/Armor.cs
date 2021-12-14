using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlot { Head, Chest, Legs, Hands, Feet, Ring, Trinket };

[CreateAssetMenu(fileName = "Armor", menuName = "ScriptableObjects/Items/Equipment/Armor")]
public class Armor : Equipment
{
    public EquipmentSlot equipmentSlot;


}
