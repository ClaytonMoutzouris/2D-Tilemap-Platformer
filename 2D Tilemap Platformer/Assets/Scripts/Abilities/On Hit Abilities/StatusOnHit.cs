using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusOnHit", menuName = "ScriptableObjects/Abilities/StatusOnHit")]
public class StatusOnHit : Ability
{
    public StatusEffect status;

    public override void OnHit(AttackData attackData, Entity hitEntity)
    {
        StatusEffect newStatus = Instantiate(status);

        newStatus.ApplyEffect(hitEntity, owner);
    }
}
