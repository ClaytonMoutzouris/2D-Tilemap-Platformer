using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Item")]
public class Item : ScriptableObject
{
    [Header("Item Info")]
    public Sprite sprite;
    public Color color;

}
