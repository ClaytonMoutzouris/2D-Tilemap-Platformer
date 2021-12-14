using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemRarity { Common, Uncommon, Rare, Epic, Legendary, Artifact };
[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/Items/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Item Info")]
    public Sprite sprite;
    public Color color;
    public ItemRarity rarity;

    public void RollRarity()
    {
        int random = Random.Range(0, 100);

        if (random < 40)
        {
            rarity = ItemRarity.Common;
        }
        else if(random < 70)
        {
            rarity = ItemRarity.Uncommon;
        }
        else if (random < 90)
        {
            rarity = ItemRarity.Rare;
        }
        else if (random < 96)
        {
            rarity = ItemRarity.Epic;
        }
        else if (random < 99)
        {
            rarity = ItemRarity.Legendary;
        }
        else
        {
            rarity = ItemRarity.Artifact;
        }
        
    }

    public virtual void RandomizeStats()
    {

    }

    public virtual string GetTooltip()
    {
        string tooltip = "";

        string itemName = name.Replace("(Clone)", "");
        tooltip += "<color=#" + ColorUtility.ToHtmlStringRGB(GetColorForRarity()) + ">" + itemName + "</color>";

        return tooltip;
    }

    public Color GetColorForRarity()
    {
        Color color = Color.white;

        switch(rarity)
        {
            case ItemRarity.Common:
                color = Color.white;
                break;
            case ItemRarity.Uncommon:
                color = Color.green;

                break;
            case ItemRarity.Rare:
                color = Color.blue;

                break;
            case ItemRarity.Epic:
                color = Color.magenta;

                break;
            case ItemRarity.Legendary:
                color = Color.yellow;

                break;
            case ItemRarity.Artifact:
                color = Color.red;

                break;
        }

        return color;
    }
}
