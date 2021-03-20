using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TilemapLayerSaveData
{
    public List<WorldTile> tiles = new List<WorldTile>();
    public int layerIndex;

    public static TilemapLayerSaveData DeepCopy(TilemapLayerSaveData original)
    {
        TilemapLayerSaveData data = new TilemapLayerSaveData();
        //make sure its empty just in case.
        data.tiles = new List<WorldTile>();

        foreach(WorldTile tile in original.tiles)
        {
            data.tiles.Add(WorldTile.DeepCopy(tile));
        }

        data.layerIndex = original.layerIndex;

        return data;
    }
}
