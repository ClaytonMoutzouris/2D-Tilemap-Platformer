using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


//Possibly a place to set options and their defaults
//Might just use a bunch of enums for things with set number of options, we'll see
[CreateAssetMenu(fileName = "ArcadeMenuOptions", menuName = "ScriptableObjects/GameData/ArcadeMenuOptions")]
public class ArcadeMenuOptions : ScriptableObject
{
    public List<int> livesOptions;
    public List<int> timeLimits;
    public List<string> maps;

    public void LoadOptions()
    {
        LoadMaps();
    }

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