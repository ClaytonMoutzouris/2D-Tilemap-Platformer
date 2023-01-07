using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ConsumableDisplay : MonoBehaviour
{
    public Image background;
    public Image itemImage;

    public void SetItem(ConsumableItem item)
    {
        if (!item)
        {
            itemImage.sprite = null;
            return;
        }

        itemImage.sprite = item.sprite;
    }

    public void ClearSlot()
    {
        itemImage.sprite = null;
    }
}
