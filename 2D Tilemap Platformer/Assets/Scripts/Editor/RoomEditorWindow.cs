using System.IO;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;


public class RoomEditorWindow : EditorWindow
{
    string roomName = "Room1";
    WorldType worldType = WorldType.Forest;
    RoomGeneralType generalType = RoomGeneralType.Underground;

    RoomData roomData = new RoomData();
    bool OpenTop = false;
    bool OpenBottom = false;
    bool OpenEast = false;
    bool OpenWest = false;
    GameGrid gameGrid;

    public void OnEnable()
    {
        gameGrid = GameObject.FindGameObjectWithTag("GameGrid").GetComponent<GameGrid>();
    }

    [MenuItem("Window/Gamble Utilities/Room Editor")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(RoomEditorWindow));
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        roomName = EditorGUILayout.TextField("Room Name", roomName);
        string[] worldTypeOptions = System.Enum.GetNames(typeof(WorldType));
        string[] roomTypeOptions = System.Enum.GetNames(typeof(RoomGeneralType));

        worldType = (WorldType)EditorGUILayout.Popup("World Type", (int)worldType, worldTypeOptions);
        generalType = (RoomGeneralType)EditorGUILayout.Popup("World Type", (int)generalType, roomTypeOptions);


        OpenTop = EditorGUILayout.Toggle("Top Opening", OpenTop);
        OpenBottom = EditorGUILayout.Toggle("Bottom Opening", OpenBottom);
        OpenEast = EditorGUILayout.Toggle("East Opening", OpenEast);
        OpenWest = EditorGUILayout.Toggle("West Opening", OpenWest);


        if (GUILayout.Button("New Room"))
        {
            NewRoom();
        }

        if (GUILayout.Button("Load Room"))
        {
            string path = EditorUtility.OpenFilePanel("Open Room File", Path.Combine(Application.streamingAssetsPath, "GameData", "Rooms"), "room");
            if (path.Length != 0)
            {
                LoadRoom(path);
            }
        }

        if (GUILayout.Button("Save Room"))
        {
            string path = EditorUtility.SaveFilePanel("Save Room to File", Path.Combine(Application.streamingAssetsPath, "GameData", "Rooms"), roomName, "room");
            if (path.Length != 0)
            {
                SaveRoom(path);
                //RoomDatabase.reload = true;
            }
        }

    }


    public void SaveRoom(string path)
    {
        RoomData saveData = new RoomData();

        for (int i = 0; i < gameGrid.tilemaps.Length; i++)
        {
            TilemapLayerSaveData layerData = new TilemapLayerSaveData();
            layerData.layerIndex = i;
            layerData.tiles = gameGrid.GetWorldTiles(i);
            saveData.mapLayers.Add(layerData);
            saveData.generalType = generalType;
            saveData.openings[0] = OpenTop;
            saveData.openings[1] = OpenBottom;
            saveData.openings[2] = OpenEast;
            saveData.openings[3] = OpenWest;
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
                gameGrid.SetWorldTiles(layerData.layerIndex, layerData.tiles, true);

            }

            OpenTop = loadData.openings[0];
            OpenBottom = loadData.openings[1];
            OpenEast = loadData.openings[2];
            OpenWest = loadData.openings[3];

            generalType = loadData.generalType;

            roomName = Path.GetFileNameWithoutExtension(path);
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

        roomData = new RoomData();

    }
}