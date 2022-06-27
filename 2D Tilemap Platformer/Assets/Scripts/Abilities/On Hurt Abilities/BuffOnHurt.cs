using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffOnHurt", menuName = "ScriptableObjects/Abilities/BuffOnHurt")]
public class BuffOnHurt : Ability
{
    public StatusEffect status;

    public override void OnHurt(AttackData attackData)
    {
        StatusEffect newStatus = Instantiate(status);

        newStatus.ApplyEffect(owner);
    }
}
