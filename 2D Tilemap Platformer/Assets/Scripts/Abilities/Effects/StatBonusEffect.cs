using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatBonusEffect", menuName = "ScriptableObjects/Effects/StatBonusEffect")]
public class StatBonusEffect : Effect
{
    public List<StatBonus> statBonuses;
    public List<SecondaryStatBonus> secondaryBonusStats;
    public List<AbilityFlagBonus> abilityFlagBonuses;


    public override void ApplyEffect(Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(effected, data);
        //Run the coroutine on the entity

        //effectedEntity.StartCoroutine(HandleEffect());
        if (effected is CharacterEntity character)
        {
            character.stats.AddPrimaryBonuses(statBonuses);
            character.stats.AddSecondaryBonuses(secondaryBonusStats);
            character.stats.AddAbilityFlagBonuses(abilityFlagBonuses);

            character.health.UpdateHealth();
        }

        effected.continuousEffects.Add(this);

        //overriding this should cover any instantaneous effects, right?
    }

    public override void RemoveEffect()
    {
        if (effectOwner is CharacterEntity character)
        {
            character.stats.RemovePrimaryBonuses(statBonuses);
            character.stats.RemoveSecondaryBonuses(secondaryBonusStats);
            character.stats.RemoveAbilityFlagBonuses(abilityFlagBonuses);

            character.health.UpdateHealth();
        }

        effectOwner.continuousEffects.Remove(this);


        base.RemoveEffect();
        //remove this from the list of effects, if we added it?
    }

}
