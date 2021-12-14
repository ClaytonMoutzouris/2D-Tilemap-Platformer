using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    public int mapSizeX = 100;
    public int mapSizeY = 100;

    public int numRoomsX = 10;
    public int numRoomsY = 10;

    public RoomData[,] rooms;

    //Create a new mapData given width and height (in number of rooms)
    public MapData(int roomsX = 10, int roomsY = 10)
    {
        rooms = new RoomData[roomsX, roomsY];
        mapSizeX = GambleConstants.RoomSizeX * roomsX;
        mapSizeY = GambleConstants.RoomSizeY * roomsY;
        numRoomsX = roomsX;
        numRoomsY = roomsY;
    }

    public void SetRoom(RoomData room, int x, int y)
    {
        /*
        foreach(WorldTile tile in room.tiles)
        {
            //Careful, these 10s should not be hardcoded
            tile.LocalPlace = new Vector3Int(tile.LocalPlace.x + GambleConstants.RoomSizeX * x, tile.LocalPlace.y + GambleConstants.RoomSizeY * y, tile.LocalPlace.z);
        }
        */
        
        rooms[x, y] = room;
    }
}

public class TilemapLayerData
{

}
