﻿using System.IO;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public class MapEditorWindow : EditorWindow
{
    string mapName = "Map1";

    //WorldType worldType = WorldType.Forest;
    MapData mapData = new MapData();
    //In number of rooms
    int mapsizeX = 10;
    int mapsizeY = 10;
    GameGrid gameGrid;

    public void OnEnable()
    {
        gameGrid = GameObject.FindGameObjectWithTag("GameGrid").GetComponent<GameGrid>();
    }

    [MenuItem("Window/Gamble Utilities/Window Editor")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(MapEditorWindow));
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        mapName = EditorGUILayout.TextField("Map Name", mapName);
        string[] worldTypeOptions = System.Enum.GetNames(typeof(WorldType));
        mapsizeX = EditorGUILayout.IntField("Number of Rooms Wide: ", mapsizeX);
        mapsizeY = EditorGUILayout.IntField("Number of Rooms High: ", mapsizeY);

        if (GUILayout.Button("New Map"))
        {
            NewMap();
        }

        if (GUILayout.Button("Generate Map"))
        {
            GenerateMap();
        }

        if (GUILayout.Button("Load Map"))
        {
            string path = EditorUtility.OpenFilePanel("Open Map File", Path.Combine(Application.streamingAssetsPath, "GameData", "Maps"), "map");
            if (path.Length != 0)
            {
                LoadMap(path);
            }
        }

        if (GUILayout.Button("Save Map"))
        {
            string path = EditorUtility.SaveFilePanel("Save Map to File", Path.Combine(Application.streamingAssetsPath, "GameData", "Maps"), mapName, "map");
            if (path.Length != 0)
            {
                SaveMap(path);
                //RoomDatabase.reload = true;
            }
        }

    }


    //Clears the map
    public void NewMap()
    {
        RoomDatabase.reload = true;
        gameGrid.ClearTiles();
        mapData = new MapData(mapsizeX, mapsizeY);
        gameGrid.mapSizeX = mapsizeX * GambleConstants.RoomSizeX;
        gameGrid.mapSizeY = mapsizeY * GambleConstants.RoomSizeY;
        gameGrid.ResizeMaps();

    }

    //Generates a random map using random rooms
    public void GenerateMap()
    {
        NewMap();
        MapLayout layout = MapLayoutDatabase.GetRandomMapLayout();
        mapsizeX = layout.mapSizeX;
        mapsizeY = layout.mapSizeY;

        Debug.Log("size X: " + mapsizeX + ", size y: " + mapsizeY);
        for (int x = 0; x < mapsizeX; x++)
        {
            for (int y = 0; y < mapsizeY; y++)
            {
                mapData.SetRoom(RoomDatabase.GetRoom(layout.layout[x,y], layout.generalLayout[x,y]), x, y);
            }
        }

        gameGrid.SetMap(mapData);
    }


    public void SaveMap(string path)
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

    public void LoadMap(string path)
    {

        if (File.Exists(path))
        {
            string loadJson = File.ReadAllText(path);

            RoomData loadData = JsonConvert.DeserializeObject<RoomData>(loadJson);

            foreach (TilemapLayerSaveData layerData in loadData.mapLayers)
            {
                gameGrid.SetWorldTiles(layerData.layerIndex, layerData.tiles, true);

            }

        }
        else
        {
            Debug.LogError("Save file not found.");
        }
    }
}