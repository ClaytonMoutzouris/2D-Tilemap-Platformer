using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusOnHit", menuName = "ScriptableObjects/Abilities/StatusOnHit")]
public class StatusOnHit : Ability
{
    public StatusEffect status;


    public List<StatusEffect> possibleEffects;

    public override void OnHit(AttackHitData hitData)
    {
        StatusEffect newStatus = Instantiate(status);

        if (hitData.hit is Entity entity)
        {
            newStatus.ApplyEffect(entity, owner);
        }
    }

    public override void RollAbility()
    {
        base.RollAbility();

        if(possibleEffects.Count > 0)
        {
            int r = Random.Range(0, possibleEffects.Count);

            status = Instantiate(possibleEffects[r]);
        }

    }

    public override string GetTooltip()
    {
        string tooltip = "";

        tooltip += status.name + " Attacks";
        tooltip += tooltip.Replace("(Clone)", "");

        return tooltip;
    }
}
