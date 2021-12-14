using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapGenerator
{

    public static MapData GenerateWorldMap()
    {
        RoomDatabase.reload = true;

        MapLayout layout = MapLayoutDatabase.GetRandomMapLayout();

        MapData mapData = new MapData(layout.mapSizeX, layout.mapSizeY);

        for (int x = 0; x < mapData.numRoomsX; x++)
        {
            for (int y = 0; y < mapData.numRoomsY; y++)
            {
                mapData.SetRoom(RoomDatabase.GetRoom(layout.layout[x, y], layout.generalLayout[x, y]), x, y);
            }
        }

        return mapData;
    }

}
