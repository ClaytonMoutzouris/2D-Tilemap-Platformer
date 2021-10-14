using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/Items/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Item Info")]
    public Sprite sprite;
    public Color color;

}
