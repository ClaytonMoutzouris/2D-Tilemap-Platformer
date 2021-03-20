using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapSaveData
{
    //Class for saving and storing whole maps
    //This is for maps that won't be generated, like the hub or a boss room.

    public List<TilemapLayerSaveData> mapLayers = new List<TilemapLayerSaveData>();

    public MapSaveData()
    {
        mapLayers = new List<TilemapLayerSaveData>();
    }
}
