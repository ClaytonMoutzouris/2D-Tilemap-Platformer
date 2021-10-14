using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GambleObjectTile", menuName = "Tiles/GambleObjectTile")]
public class GambleObjectTileBase : GambleTileBase
{
    //This is the object that will spawn in place of the tile (runtime only)
    public GameObject spawnObject;

}
