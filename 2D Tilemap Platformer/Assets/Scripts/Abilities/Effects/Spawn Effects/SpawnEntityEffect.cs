using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnEntityEffect", menuName = "ScriptableObjects/Effects/SpawnEffects/SpawnEntityEffect")]
public class SpawnEntityEffect : ImmediateEffect
{
    public Entity spawnPrefab;

    public override void Apply(Entity effected, Entity effector = null)
    {
        base.Apply(effected, effector);

        Instantiate(spawnPrefab, effected.transform.position, Quaternion.identity);
    }

}
