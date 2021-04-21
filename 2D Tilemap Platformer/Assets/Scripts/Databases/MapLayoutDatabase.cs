using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public static class MapLayoutDatabase
{
    public static List<MapLayout> MapLayouts = new List<MapLayout>();
    public static bool reload = true;

    static void CheckDatabase()
    {
        if (reload)
        {
            LoadMapLayouts();
            reload = false;
        }
    }

    //Loads MapLayouts into memory for parsing
    public static void LoadMapLayouts()
    {

        foreach (string MapLayout in Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "GameData", "Layouts"), "*.layout"))
        {
            if (File.Exists(MapLayout))
            {
                string loadJson = File.ReadAllText(MapLayout);

                MapLayout loadData = JsonConvert.DeserializeObject<MapLayout>(loadJson);
                MapLayouts.Add(loadData);
            }
            else
            {
                Debug.LogError("MapLayout file not found: " + MapLayout);
            }
        }
    }

    public static MapLayout GetRandomMapLayout()
    {
        CheckDatabase();

        if (MapLayouts.Count > 0)
        {
            return MapLayouts[Random.Range(0, MapLayouts.Count)];
        }

        return null;
    }
}
