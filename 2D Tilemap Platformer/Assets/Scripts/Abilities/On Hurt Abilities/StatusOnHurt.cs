using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusOnHurt", menuName = "ScriptableObjects/Abilities/StatusOnHurt")]
public class StatusOnHurt : Ability
{
    public StatusEffect status;

    public override void OnHurt(AttackData attackData)
    {
        StatusEffect newStatus = Instantiate(status);

        newStatus.ApplyEffect(attackData.owner, owner);
    }
}
