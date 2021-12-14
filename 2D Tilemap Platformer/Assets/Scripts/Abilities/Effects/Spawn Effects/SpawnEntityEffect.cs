using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnEntityEffect", menuName = "ScriptableObjects/Effects/SpawnEffects/SpawnEntityEffect")]
public class SpawnEntityEffect : Effect
{
    public Entity spawnPrefab;

    public override void ApplyEffect()
    {
        Instantiate(spawnPrefab, effected.transform.position, Quaternion.identity);

    }
}
