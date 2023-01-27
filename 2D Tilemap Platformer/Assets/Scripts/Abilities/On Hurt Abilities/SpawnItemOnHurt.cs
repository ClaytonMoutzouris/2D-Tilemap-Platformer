using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnItemOnHurt", menuName = "ScriptableObjects/Abilities/SpawnItemOnHurt")]
public class SpawnItemOnHurt : Ability
{
    public ItemData itemData;
    public ItemObject prefab;
    //Could probably include more stuff here
    //Such as how the projectile is aimed/give it different attack data etc.

    public override void OnHurt(AttackHitData hitData)
    {
        ItemObject newItem = Instantiate(prefab, owner.transform.position, Quaternion.identity);
        ItemData newData = Instantiate(itemData);
        
        newData.RandomizeStats();
        newItem.SetItem(newData);
    }
}