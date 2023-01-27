using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnItemOnHit", menuName = "ScriptableObjects/Abilities/SpawnItemOnHit")]
public class SpawnItemOnHit : Ability
{
    public ItemData itemData;
    public ItemObject prefab;
    //Could probably include more stuff here
    //Such as how the projectile is aimed/give it different attack data etc.

    public override void OnHit(AttackHitData hitData)
    {
        //This spawns it out of the hit entity
        if(hitData.hit is Entity entity)
        {
            ItemObject newItem = Instantiate(prefab, entity.transform.position, Quaternion.identity);
            ItemData newData = Instantiate(itemData);

            newData.RandomizeStats();
            newItem.SetItem(newData);
        }

    }
}