using System.IO;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;


public class RoomEditorWindow : EditorWindow
{
    string roomName = "Room1";
    WorldType worldType = WorldType.Forest;
    RoomData roomData = new RoomData();
    bool OpenTop = false;
    bool OpenBottom = false;
    bool OpenLeft = false;
    bool OpenRight = false;
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

        worldType = (WorldType)EditorGUILayout.Popup("World Type", (int)worldType, worldTypeOptions);


        OpenTop = EditorGUILayout.Toggle("Top Opening", OpenTop);
        OpenBottom = EditorGUILayout.Toggle("Bottom Opening", OpenBottom);
        OpenLeft = EditorGUILayout.Toggle("Left Opening", OpenLeft);
        OpenRight = EditorGUILayout.Toggle("Right Opening", OpenRight);


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
            saveData.openings[0] = OpenTop;
            saveData.openings[1] = OpenBottom;
            saveData.openings[2] = OpenLeft;
            saveData.openings[3] = OpenRight;
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
            OpenLeft = loadData.openings[2];
            OpenRight = loadData.openings[3];

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