using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GainStatus", menuName = "ScriptableObjects/Effects/GainStatus")]
public class GainStatus : Effect
{
    public StatusEffect status;

    public override void ApplyEffect(Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(effected, data);

        if (data != null)
        {
            StatusEffect newStatus = Instantiate(status);

            status.ApplyEffect(effectOwner);
        }

    }
}
