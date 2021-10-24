using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    // Start is called before the first frame update
    void Awake()
    {
        ArenaBattleManager.instance.spawnPoints.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetToTile(int x, int y)
    {
        transform.position = new Vector2(x, y);
    }
}
