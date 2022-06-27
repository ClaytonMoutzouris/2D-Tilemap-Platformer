using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffOnHit", menuName = "ScriptableObjects/Abilities/BuffOnHit")]
public class BuffOnHit : Ability
{
    public StatusEffect status;

    public override void OnHit(AttackData attackData, Entity hitEntity)
    {
        StatusEffect newStatus = Instantiate(status);

        newStatus.ApplyEffect(owner);
    }
}
