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
        if (abilities.Count > 0)
        {
            int r = Random.Range(0, abilities.Count);

            return ScriptableObject.Instantiate(abilities[r]);
        }

        return null;
    }
}
