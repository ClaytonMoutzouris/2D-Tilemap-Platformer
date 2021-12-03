using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnNode : MonoBehaviour
{
    public ItemObject prefab;
    public List<ItemData> items;
    ItemObject itemEntity;
    public float spawnTime = 1;

    void Start()
    {

        SpawnItem();


    }

    public void SpawnItem()
    {
        
        itemEntity = Instantiate(prefab);
        itemEntity.transform.position = transform.position;
        itemEntity.SetSpawner(this);
        itemEntity.SetItem(GetRandomItem());
    }

    public void ItemCollected()
    {
        StartCoroutine(Respawn());
    }

    public IEnumerator Respawn()
    {

        yield return new WaitForSeconds(spawnTime);
        SpawnItem();
        //gameObject.SetActive(true);

    }

    public ItemData GetRandomItem()
    {
        int r = Random.Range(0, items.Count);

        return Instantiate(items[r]);
    }
}
