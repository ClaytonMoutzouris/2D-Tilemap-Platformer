using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapLayout
{
    public RoomAccessType[,] layout;
    public RoomGeneralType[,] generalLayout;
    public int mapSizeX = 10;
    public int mapSizeY = 10;
    
    public MapLayout(int x = 10, int y = 10)
    {
        mapSizeX = x;
        mapSizeY = y;

    }

    public void LayoutFromTiles(List<WorldTile> tiles)
    {
        layout = new RoomAccessType[mapSizeX, mapSizeY];

        foreach (WorldTile tile in tiles)
        {
            layout[tile.LocalPlace.x, tile.LocalPlace.y] = (RoomAccessType)System.Enum.Parse(typeof(RoomAccessType), tile.TileID);

        }

    }

    public void GeneralLayoutFromTiles(List<WorldTile> tiles)
    {
        generalLayout = new RoomGeneralType[mapSizeX, mapSizeY];

        foreach (WorldTile tile in tiles)
        {
            generalLayout[tile.LocalPlace.x, tile.LocalPlace.y] = (RoomGeneralType)System.Enum.Parse(typeof(RoomGeneralType), tile.TileID);
        }
    }

}
