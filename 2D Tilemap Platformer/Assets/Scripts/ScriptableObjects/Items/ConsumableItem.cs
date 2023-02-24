using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "ScriptableObjects/Items/Consumables/Consumable")]
public class ConsumableItem : ItemData
{
    public Effect consumeEffect;

    public virtual bool Use(PlayerController entity)
    {
        //destroy it/remove it from inventory?

        Effect temp = Instantiate(consumeEffect);

        temp.ApplyEffect(entity);

        return true;
    }
}
