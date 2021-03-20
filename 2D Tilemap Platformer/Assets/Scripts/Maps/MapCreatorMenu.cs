using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class MapCreatorMenu : MonoBehaviour
{
    public string saveName;
    public string folderName;
    public GameGrid gameGrid;

    public void SaveMap()
    {
        MapSaveData saveData = new MapSaveData();

        for (int i = 0; i < gameGrid.tilemaps.Length; i++)
        {
            TilemapLayerSaveData layerData = new TilemapLayerSaveData();
            layerData.layerIndex = i;
            layerData.tiles = gameGrid.GetWorldTiles(i);
            saveData.mapLayers.Add(layerData);
        }

        string path = Path.Combine(Application.streamingAssetsPath, "GameData");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        path = Path.Combine(path, folderName);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        path = Path.Combine(path, saveName + ".map");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        string saveJson = JsonConvert.SerializeObject(saveData);

        File.WriteAllText(path, saveJson);
    }

    public void LoadMap()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "GameData");
        path = Path.Combine(path, folderName);
        path = Path.Combine(path, saveName + ".map");

        if (File.Exists(path))
        {
            string loadJson = File.ReadAllText(path);

            MapSaveData loadData = JsonConvert.DeserializeObject<MapSaveData>(loadJson);

            foreach(TilemapLayerSaveData layerData in loadData.mapLayers)
            {
                gameGrid.SetWorldTiles(layerData.layerIndex, layerData.tiles);

            }
        }
        else
        {
            Debug.LogError("Save file not found.");
        }
    }

    public void NewMap()
    {
        gameGrid.ClearTiles();
    }
}