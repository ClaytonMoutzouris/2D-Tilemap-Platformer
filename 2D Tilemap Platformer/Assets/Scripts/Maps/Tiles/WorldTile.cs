using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//This is a class used for storing tile data
[System.Serializable]
public class WorldTile
{
    public Vector3Int LocalPlace { get; set; }
    public string TileID { get; set; }
    public string SpawnObject { get; set; }
    public TileMapLayersEnum LayerID { get; set; }


    public static WorldTile DeepCopy(WorldTile original)
    {
        WorldTile data = new WorldTile();

        data.LocalPlace = original.LocalPlace;
        data.TileID = original.TileID;

        //Added for "Gamble" tiles
        data.SpawnObject = original.SpawnObject;
        data.LayerID = original.LayerID;

        return data;
    }
}

[System.Serializable]
public class WorldObjectTile : WorldTile
{

    public static WorldObjectTile DeepCopy(WorldObjectTile original)
    {
        WorldObjectTile data = new WorldObjectTile();

        data.LocalPlace = original.LocalPlace;
        data.TileID = original.TileID;

        //Added for "Gamble" tiles
        data.SpawnObject = original.SpawnObject;
        data.LayerID = original.LayerID;

        return data;
    }
}