using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "ScriptableObjects/Items/Consumables/Consumable")]
public class ConsumableItem : ItemData
{

    public virtual bool Use(PlayerController entity)
    {
        //destroy it/remove it from inventory?

        return true;
    }
}
