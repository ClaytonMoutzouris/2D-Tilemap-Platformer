using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InflictStatus", menuName = "ScriptableObjects/Effects/InflictStatus")]
public class InflictStatus : Effect
{

    public StatusEffect status;

    public override void ApplyEffect(Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(effected, data);

        if (data != null)
        {
            StatusEffect newStatus = Instantiate(status);

            status.ApplyEffect(data.hit.GetEntity(), data.attackOwner);
        }

    }

}
