using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatChangeEffect", menuName = "ScriptableObjects/StatusEffects/StatChangeEffect")]
public class StatChangeEffect : StatusEffect
{
    public List<StatBonus> statChanges;

    public override void StartUp()
    {
        base.StartUp();

        effectedEntity.stats.AddPrimaryBonuses(statChanges);
    }

    public override void EffectEnd()
    {
        effectedEntity.stats.RemovePrimaryBonuses(statChanges);

        base.EffectEnd();
    }

}
