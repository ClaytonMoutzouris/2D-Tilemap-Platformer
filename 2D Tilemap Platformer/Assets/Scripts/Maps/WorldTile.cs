using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class WorldTile
{
    public Vector3Int LocalPlace { get; set; }
    public string TileID { get; set; }

    public static WorldTile DeepCopy(WorldTile original)
    {
        WorldTile data = new WorldTile();

        data.LocalPlace = original.LocalPlace;
        data.TileID = original.TileID;

        return data;
    }
}