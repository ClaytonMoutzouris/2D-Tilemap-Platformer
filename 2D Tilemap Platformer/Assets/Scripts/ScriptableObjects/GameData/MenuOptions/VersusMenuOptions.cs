using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//Possibly a place to set options and their defaults
//Might just use a bunch of enums for things with set number of options, we'll see
[CreateAssetMenu(fileName = "VersusMenuOptions", menuName = "ScriptableObjects/GameData/VersusMenuOptions")]
public class VersusMenuOptions : ScriptableObject
{
    public GameMode gameMode;
    public int lives;
    public int timeLimit;
    public string mapName;

    public List<GameMode> gameModes;
    public List<int> livesOptions;
    public List<int> timeLimits;
    public List<string> maps;

    public void LoadMaps()
    {
        maps.Clear();
        //MenuOptionSelectorUI menuSelectorNode = menuOptions[(int)MenuOptionIndex.Map];
        //menuSelectorNode.ClearOptions();
        string path = Path.Combine(Application.streamingAssetsPath, "GameData", "Maps", "");

        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.map");
        Debug.Log(path);

        foreach (FileInfo file in info)
        {
            Debug.Log(file.Name);
            maps.Add(Path.GetFileNameWithoutExtension(file.Name));
        }

    }
}