using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine.Serialization;
using UnityEngine;

public enum TileDataType { Decoration = -1, Empty, Ground, PlayerSpawn, ItemSpawn }

[CreateAssetMenu(fileName = "GambleTile", menuName = "Tiles/GambleTile")]
public class GambleTileBase : Tile
{

    public TileMapLayersEnum tileType;
    //public WorldType worldType;

}
