using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawnNode : MonoBehaviour
{
    public RollingRock prefab;
    public EntityDirection direction = EntityDirection.Right;
    RollingRock rockEntity;

    void Awake()
    {
        //ArenaBattleManager.instance.chestSpawnPoints.Add(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        SpawnRock();

    }

    public void SpawnRock()
    {

        rockEntity = Instantiate(prefab);
        rockEntity.transform.position = transform.position;
        rockEntity.SetSpawner(this);

    }

    public void RockDestroyed()
    {
        StartCoroutine(Respawn());
    }

    public IEnumerator Respawn()
    {

        yield return new WaitForSeconds(0.5f);
        SpawnRock();
        //gameObject.SetActive(true);

    }

    public void SetToTile(int x, int y)
    {
        transform.position = new Vector2(x, y);
    }
}
