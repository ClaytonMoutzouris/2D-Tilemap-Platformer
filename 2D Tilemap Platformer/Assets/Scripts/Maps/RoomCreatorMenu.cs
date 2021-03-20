using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class RoomCreatorMenu : MonoBehaviour
{
    public string saveName;
    public string folderName;

    public int Width = 10;
    public int Height = 10;



    public GameGrid gameGrid;

    public void SaveRoom(string path)
    {
        RoomData saveData = new RoomData();

        for (int i = 0; i < gameGrid.tilemaps.Length; i++)
        {
            TilemapLayerSaveData layerData = new TilemapLayerSaveData();
            layerData.layerIndex = i;
            layerData.tiles = gameGrid.GetWorldTiles(i);
            saveData.mapLayers.Add(layerData);

        }

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        string saveJson = JsonConvert.SerializeObject(saveData);

        File.WriteAllText(path, saveJson);
    }

    public void LoadRoom(string path)
    {

        if (File.Exists(path))
        {
            string loadJson = File.ReadAllText(path);

            RoomData loadData = JsonConvert.DeserializeObject<RoomData>(loadJson);

            foreach (TilemapLayerSaveData layerData in loadData.mapLayers)
            {
                gameGrid.SetWorldTiles(layerData.layerIndex, layerData.tiles);

            }

        }
        else
        {
            Debug.LogError("Save file not found.");
        }
    }

    public void NewRoom()
    {
        gameGrid.ClearTiles();
        gameGrid.mapSizeX = GambleConstants.RoomSizeX;
        gameGrid.mapSizeY = GambleConstants.RoomSizeY;
        gameGrid.ResizeMaps();
    }
}