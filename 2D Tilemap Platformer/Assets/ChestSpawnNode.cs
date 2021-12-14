using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawnNode : MonoBehaviour
{
    public Chest prefab;
    Chest chestEntity;
    public float spawnTime = 5;

    void Awake()
    {
        ArenaBattleManager.instance.chestSpawnPoints.Add(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        //SpawnChest();

    }

    public void SpawnChest()
    {

        chestEntity = Instantiate(prefab);
        chestEntity.transform.position = transform.position;
        chestEntity.SetSpawner(this);
    }

    public void ChestOpened()
    {
        //StartCoroutine(Respawn());
        ArenaBattleManager.instance.ChestCollected();

    }

    public IEnumerator Respawn()
    {

        yield return new WaitForSeconds(spawnTime);
        //ArenaBattleManager.instance.SpawnChest();
        //gameObject.SetActive(true);

    }

    public void SetToTile(int x, int y)
    {
        transform.position = new Vector2(x, y);
    }
}
