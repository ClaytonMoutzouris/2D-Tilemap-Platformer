using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBeltUI : MonoBehaviour
{
    public ConsumableDisplay[] beltSlots;

    public void SetItem(ConsumableItem item, int index)
    {
        beltSlots[index].SetItem(item);
    }

    public ConsumableDisplay GetNextOpenSlot()
    {
        for(int i = 0; i < beltSlots.Length; i++)
        {
            if(!beltSlots[i])
            {
                return beltSlots[i];
            }
        }

        return null;
    }

}
