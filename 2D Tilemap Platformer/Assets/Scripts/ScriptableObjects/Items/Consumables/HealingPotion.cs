using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : ConsumableItem
{
    public int healingValue = 10;
    public override bool Use(PlayerController entity)
    {
        //destroy it/remove it from inventory?
        if(entity.health.AtFullHealth())
        {
            return false;
        }

        entity.health.GainLife(healingValue);
        return true;
    }
}