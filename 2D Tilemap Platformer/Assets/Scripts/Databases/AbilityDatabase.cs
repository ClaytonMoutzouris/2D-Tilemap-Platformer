using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public static class AbilityDatabase
{
    public static List<Ability> abilities = new List<Ability>();
    public static bool reload = true;

    static void CheckDatabase()
    {
        if (reload)
        {
            LoadAbilities();
            reload = false;
        }
    }

    // Start is called before the first frame update
    public static void LoadAbilities()
    {

        foreach (Ability ability in Resources.LoadAll<Ability>("Prototypes/Abilities"))
        {
            abilities.Add(ability);
        }
    }

    public static Ability GetRandomAbility()
    {
        CheckDatabase();

        Ability ability = null;

        if (abilities.Count > 0)
        {
            int r = Random.Range(0, abilities.Count);
            ability = ScriptableObject.Instantiate(abilities[r]);
            ability.RollAbility();
        }

        return ability;
    }

    public static Ability GetRandomAbility(ItemRarity rarity)
    {
        CheckDatabase();

        Ability ability = null;

        List<Ability> validAbilities = GetAbilitiesForRarity(rarity);

        if (validAbilities.Count > 0)
        {
            int r = Random.Range(0, validAbilities.Count);
            ability = ScriptableObject.Instantiate(validAbilities[r]);
            ability.RollAbility();
        }

        return ability;
    }

    public static List<Ability> GetAbilitiesForRarity(ItemRarity rarity)
    {
        List<Ability> validAbilities = new List<Ability>();

        foreach(Ability ability in abilities)
        {
            if(ability.CheckRarity(rarity))
            {
                validAbilities.Add(ability);
            }
        }

        return validAbilities;

    }
}
