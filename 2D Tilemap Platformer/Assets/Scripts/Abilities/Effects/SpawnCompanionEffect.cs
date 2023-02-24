using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnCompanionEffect", menuName = "ScriptableObjects/Effects/SpawnCompanionEffect")]
public class SpawnCompanionEffect : Effect
{
    public Companion prefab;
    Companion currentCompanion;

    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(owner, effected, data);

        if (owner is PlayerController player)
        {
            Companion newCompanion = Instantiate(prefab, effected.transform.position, Quaternion.identity);
            newCompanion.SetOwner(player);
            currentCompanion = newCompanion;
        }
    }

    public override void RemoveEffect()
    {
        currentCompanion.owner._companionManager.RemoveCompanion(currentCompanion);
        currentCompanion.owner = null;

        Destroy(currentCompanion.gameObject);
        currentCompanion = null;

        base.RemoveEffect();
    }


}
