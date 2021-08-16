using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObjects/Items/Equipment/Equipment")]
public class Equipment : Item
{
    public List<Ability> generalAbilities;
    public List<StatBonus> bonusStats;
    public List<SecondaryStatBonus> secondaryBonusStats;

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
            ability.GainAbility(owner);
        }

        //Add the stat bonuses
        if(owner.stats != null)
        {
            owner.stats.AddPrimaryBonuses(bonusStats);
            owner.stats.AddSecondaryBonuses(secondaryBonusStats);
            owner.health.UpdateHealth();
        }
    }

    public virtual void OnUnequipped(Entity entity)
    {
        //Remove the abilities

        foreach (Ability ability in generalAbilities)
        {
            ability.LoseAbility(entity);
        }

        //Add the stat bonuses
        if (entity.stats != null)
        {
            entity.stats.RemovePrimaryBonuses(bonusStats);
            entity.stats.RemoveSecondaryBonuses(secondaryBonusStats);
            entity.health.UpdateHealth();
        }
    }
}
