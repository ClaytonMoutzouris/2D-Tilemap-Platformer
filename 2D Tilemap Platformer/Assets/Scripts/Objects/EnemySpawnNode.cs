using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnNode : MonoBehaviour
{
    public Entity prefab;
    //public List<Item> items;
    Entity spawnedEntity;
    public float spawnTime = 5;

    void Start()
    {

        SpawnEnemy();


    }

    public void SpawnEnemy()
    {

        spawnedEntity = Instantiate(prefab);
        spawnedEntity.transform.position = transform.position;
        //spawnedEntity.SetSpawner(this);
        //spawnedEntity.SetItem(GetRandomItem());
        StartCoroutine(Respawn());

    }


    public IEnumerator Respawn()
    {
        while(spawnedEntity != null)
        {
            yield return null;
        }

        yield return new WaitForSeconds(spawnTime);
        SpawnEnemy();
        //gameObject.SetActive(true);

    }

}
