using System.IO;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public class MapLayoutEditorWindow : EditorWindow
{
    string layoutName = "Layout1";
    WorldType worldType = WorldType.Forest;
    int mapSizeX = 10;
    int mapSizeY = 10;
    MapLayout maplayout;
    GameGrid gameGrid;

    public void OnEnable()
    {
        gameGrid = GameObject.FindGameObjectWithTag("GameGrid").GetComponent<GameGrid>();
    }

    [MenuItem("Window/Gamble Utilities/Map Layout Editor")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(MapLayoutEditorWindow));
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        layoutName = EditorGUILayout.TextField("Layout Name", layoutName);
        string[] worldTypeOptions = System.Enum.GetNames(typeof(WorldType));
        mapSizeX = EditorGUILayout.IntField("Map Size X: ", mapSizeX);
        mapSizeY = EditorGUILayout.IntField("Map Size Y: ", mapSizeY);
   
        worldType = (WorldType)EditorGUILayout.Popup("World Type", (int)worldType, worldTypeOptions);

        if (GUILayout.Button("New Layout"))
        {
            NewLayout();
        }

        if (GUILayout.Button("Load Layout"))
        {
            string path = EditorUtility.OpenFilePanel("Open Layout File", Path.Combine(Application.streamingAssetsPath, "GameData", "Layouts"), "layout");
            if (path.Length != 0)
            {
                LoadLayout(path);
            }
        }

        if (GUILayout.Button("Save Layout"))
        {
            string path = EditorUtility.SaveFilePanel("Save Layout to File", Path.Combine(Application.streamingAssetsPath, "GameData", "Layouts"), layoutName, "layout");
            if (path.Length != 0)
            {
                SaveLayout(path);
                MapLayoutDatabase.reload = true;
            }
        }

    }


    public void SaveLayout(string path)
    {
        MapLayout saveData = new MapLayout(mapSizeX, mapSizeY);
        saveData.mapSizeX = mapSizeX;
        saveData.mapSizeY = mapSizeY;
        saveData.LayoutFromTiles(gameGrid.GetWorldTiles(0));
        saveData.GeneralLayoutFromTiles(gameGrid.GetWorldTiles(1));

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        string saveJson = JsonConvert.SerializeObject(saveData);

        File.WriteAllText(path, saveJson);
    }

    public void LoadLayout(string path)
    {

        if (File.Exists(path))
        {
            string loadJson = File.ReadAllText(path);

            MapLayout loadData = JsonConvert.DeserializeObject<MapLayout>(loadJson);
            maplayout = loadData;
            mapSizeX = loadData.mapSizeX;
            mapSizeY = loadData.mapSizeY;

            List<WorldTile> worldTiles = new List<WorldTile>();
            for(int x = 0; x < mapSizeX; x++)
            {
                for (int y = 0; y < mapSizeY; y++)
                {
                    WorldTile temp = new WorldTile();
                    temp.LocalPlace = new Vector3Int(x, y, 0);
                    temp.TileID = maplayout.layout[x, y].ToString();
                    worldTiles.Add(temp);
                }
            }

            gameGrid.SetWorldTiles(0, worldTiles);

            worldTiles = new List<WorldTile>();
            for (int x = 0; x < mapSizeX; x++)
            {
                for (int y = 0; y < mapSizeY; y++)
                {
                    WorldTile temp = new WorldTile();
                    temp.LocalPlace = new Vector3Int(x, y, 0);
                    temp.TileID = maplayout.generalLayout[x, y].ToString();
                    worldTiles.Add(temp);
                }
            }

            gameGrid.SetWorldTiles(1, worldTiles);
        }
        else
        {
            Debug.LogError("Save file not found.");
        }
    }

    public void NewLayout()
    {
        gameGrid.ClearTiles();
        gameGrid.mapSizeX = mapSizeX;
        gameGrid.mapSizeY = mapSizeY;
        gameGrid.ResizeMaps();

        maplayout = new MapLayout(mapSizeX, mapSizeY);
        
    }
}