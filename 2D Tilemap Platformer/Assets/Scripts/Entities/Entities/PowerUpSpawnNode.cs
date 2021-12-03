using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawnNode : MonoBehaviour
{
    public PowerUpObject prefab;
    public List<PowerUp> powerUps;
    PowerUpObject powerupEntity;
    public float spawnTime = 20;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPowerUp();

    }

    public void SpawnPowerUp()
    {

        powerupEntity = Instantiate(prefab);
        powerupEntity.transform.position = transform.position;
        powerupEntity.SetSpawner(this);

        powerupEntity.SetEffect(GetRandomPowerup());
    }

    public void Collected()
    {
        StartCoroutine(Respawn());
    }

    public IEnumerator Respawn()
    {

        yield return new WaitForSeconds(spawnTime);
        SpawnPowerUp();
        //gameObject.SetActive(true);

    }

    public PowerUp GetRandomPowerup()
    {
        int r = Random.Range(0, powerUps.Count);

        return Instantiate(powerUps[r]);
    }
}
