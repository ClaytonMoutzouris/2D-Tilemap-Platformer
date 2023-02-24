using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GainStatus", menuName = "ScriptableObjects/Effects/GainStatus")]
public class GainStatus : Effect
{
    public StatusEffect status;

    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(owner, effected, data);

        StatusEffect newStatus = Instantiate(status);
        status.ApplyEffect(effectedEntity);
        

    }
}
