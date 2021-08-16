using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnNode : Entity
{
    public ItemEntity prefab;
    public Item item;
    ItemEntity itemEntity;

    void Start()
    {

        SpawnItem();


    }

    public void SpawnItem()
    {
        
        itemEntity = Instantiate(prefab);
        itemEntity.transform.position = transform.position;
        itemEntity.SetSpawner(this);
        itemEntity.SetItem(item);
    }

    public void ItemCollected()
    {
        StartCoroutine(Respawn());
    }

    public IEnumerator Respawn()
    {

        yield return new WaitForSeconds(5.0f);
        SpawnItem();
        //gameObject.SetActive(true);

    }
}
