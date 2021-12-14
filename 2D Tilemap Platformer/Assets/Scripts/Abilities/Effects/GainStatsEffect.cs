using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainStatsEffect : Effect
{
    public List<StatBonus> statBonuses;
    public List<SecondaryStatBonus> secondaryStatBonus;
    public List<WeaponAttributeBonus> weaponBonuses;
    public List<AbilityFlagBonus> abilityFlagBonuses;



    public override void ApplyEffect()
    {
        if (effected.stats != null)
        {
            effected.stats.AddPrimaryBonuses(statBonuses);
            effected.stats.AddSecondaryBonuses(secondaryStatBonus);
            effected.stats.AddAbilityFlagBonuses(abilityFlagBonuses);

        }
    }


    public void RemoveEffect()
    {

        if (effected.stats != null)
        {
            effected.stats.RemovePrimaryBonuses(statBonuses);
            effected.stats.RemoveSecondaryBonuses(secondaryStatBonus);
            effected.stats.RemoveAbilityFlagBonuses(abilityFlagBonuses);

        }
    }

}
