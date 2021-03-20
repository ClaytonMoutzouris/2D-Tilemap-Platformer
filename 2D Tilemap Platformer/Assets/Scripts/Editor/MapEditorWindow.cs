using System.IO;
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

    List<RoomData> rooms = new List<RoomData>();

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

    }


    //Clears the map
    public void NewMap()
    {
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
        rooms = new List<RoomData>();
        LoadRooms();


        for (int x = 0; x < mapsizeX; x++)
        {
            for (int y = 0; y < mapsizeY; y++)
            {
                mapData.SetRoom(RoomData.DeepCopy(rooms[Random.Range(0, rooms.Count)]), x, y);
            }
        }

        gameGrid.SetMap(mapData);
    }

    //Loads rooms into memory for parsing
    public void LoadRooms()
    {
        foreach (string room in Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "GameData", "Rooms"), "*.room"))
        {
            if (File.Exists(room))
            {
                string loadJson = File.ReadAllText(room);

                RoomData loadData = JsonConvert.DeserializeObject<RoomData>(loadJson);
                rooms.Add(loadData);
            }
            else
            {
                Debug.LogError("Room file not found: " + room);
            }
        }
    }
}