using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatChangeEffect", menuName = "ScriptableObjects/StatusEffects/StatChangeEffect")]
public class StatChangeEffect : StatusEffect
{
    public List<StatBonus> statChanges;
    public List<SecondaryStatBonus> secondaryStatChanges;

    public override void StartUp()
    {
        base.StartUp();
        if(effectedEntity is CharacterEntity character)
        {
            character.stats.AddPrimaryBonuses(statChanges);
            character.stats.AddSecondaryBonuses(secondaryStatChanges);
        }

    }

    public override void EffectEnd()
    {
        if (effectedEntity is CharacterEntity character)
        {
            character.stats.RemovePrimaryBonuses(statChanges);
            character.stats.RemoveSecondaryBonuses(secondaryStatChanges);
        }

        base.EffectEnd();
    }

}
