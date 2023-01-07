using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObjects/Items/Equipment/Equipment")]
public class Equipment : ItemData
{
    public List<Ability> generalAbilities;
    public List<StatBonus> bonusStats;
    public List<SecondaryStatBonus> secondaryBonusStats;
    public List<AbilityFlagBonus> abilityFlagBonuses;

    [HideInInspector]
    public PlayerController owner;

    public virtual void OnEquipped(PlayerController entity)
    {
        owner = entity;
        List<Ability> temp = new List<Ability>();

        foreach (Ability ability in generalAbilities)
        {
            temp.Add(Instantiate(ability));
        }

        generalAbilities = temp;

        //Add the abilities
        foreach (Ability ability in generalAbilities)
        {
            ability.OnGainedAbility(owner);
        }

        //Add the stat bonuses
        if(owner.stats != null)
        {
            owner.stats.AddPrimaryBonuses(bonusStats);
            owner.stats.AddSecondaryBonuses(secondaryBonusStats);
            owner.stats.AddAbilityFlagBonuses(abilityFlagBonuses);
            owner.health.UpdateHealth();
        }
    }

    public virtual void OnUnequipped(Entity entity)
    {
        //Remove the abilities

        foreach (Ability ability in generalAbilities)
        {
            ability.OnAbilityLost();
        }

        //Add the stat bonuses
        if (entity.stats != null)
        {
            entity.stats.RemovePrimaryBonuses(bonusStats);
            entity.stats.RemoveSecondaryBonuses(secondaryBonusStats);
            owner.stats.RemoveAbilityFlagBonuses(abilityFlagBonuses);
            entity.health.UpdateHealth();
        }
    }


    public override void RandomizeStats()
    {
        RollRarity();

        int statMin = 0;
        int statMax = 0;

        int numStatBonuses = 0;

        switch(rarity)
        {
            case ItemRarity.Common:
                statMin = 0;
                statMax = 0;

                numStatBonuses = 0;

                break;
            case ItemRarity.Uncommon:
                statMin = 1;
                statMax = 2;

                numStatBonuses = Random.Range(1, 2);
                break;
            case ItemRarity.Rare:
                statMin = 1;
                statMax = 2;
                generalAbilities.Add(AbilityDatabase.GetRandomAbility());
                numStatBonuses = Random.Range(1, 3);
                break;
            case ItemRarity.Epic:
                statMin = 1;
                statMax = 3;
                generalAbilities.Add(AbilityDatabase.GetRandomAbility());

                numStatBonuses = Random.Range(2, 4);
                break;
            case ItemRarity.Legendary:
                statMin = 1;
                statMax = 3;
                generalAbilities.Add(AbilityDatabase.GetRandomAbility());

                numStatBonuses = Random.Range(3, 5);
                break;
            case ItemRarity.Artifact:
                statMin = 1;
                statMax = 4;
                generalAbilities.Add(AbilityDatabase.GetRandomAbility());

                numStatBonuses = Random.Range(4, 6);
                break;
        }


        for(int i = 0; i < numStatBonuses; i++)
        {
            bonusStats.Add(new StatBonus((StatType)Random.Range(0, 5), Random.Range(statMin, statMax)));
        }



    }


    public override string GetTooltip()
    {
        string tooltip = base.GetTooltip();

        foreach (Ability ability in generalAbilities)
        {
            tooltip += "\n<color=#00FFFF>" + ability.GetTooltip() + "</color>";
        }

        foreach (StatBonus bonus in bonusStats)
        {
            tooltip += "\n<color=white>" + bonus.type + " +" + bonus.bonusValue + "</color>";
        }

        foreach (SecondaryStatBonus secBonus in secondaryBonusStats)
        {
            tooltip += "\n<color=green>" + secBonus.type + " +" + secBonus.bonusValue + "</color>";
        }

        return tooltip;
    }
}
