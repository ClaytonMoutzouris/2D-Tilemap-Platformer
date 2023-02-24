using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatChangeEffect", menuName = "ScriptableObjects/StatusEffects/StatChangeEffect")]
public class StatChangeEffect : StatusEffect
{
    public List<StatBonus> statChanges;
    public List<SecondaryStatBonus> secondaryStatChanges;


    public override void RemoveEffect()
    {
        if (effectedEntity is CharacterEntity character)
        {
            character.stats.RemovePrimaryBonuses(statChanges);
            character.stats.RemoveSecondaryBonuses(secondaryStatChanges);
        }

        base.RemoveEffect();
    }

    public override IEnumerator HandleEffect()
    {
        if (effectedEntity is CharacterEntity character)
        {
            character.stats.AddPrimaryBonuses(statChanges);
            character.stats.AddSecondaryBonuses(secondaryStatChanges);
        }

        timeStamp = Time.time;

        while (unlimitedDuration || Time.time < timeStamp + duration)
        {
            if (!effectedEntity)
            {
                break;
            }

            yield return null;
        }

        RemoveEffect();
    }
}
