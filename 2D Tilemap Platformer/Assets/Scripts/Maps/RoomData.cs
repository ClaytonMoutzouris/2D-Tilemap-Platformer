using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RoomOpening { North = 0, South = 1, East = 2, West = 3, Count }
[System.Serializable]
public enum RoomAccessType { Empty = 0, Closed = 1, NorthSouthEastWest, NorthEastWest, NorthSouthWest, NorthSouthEast, SouthEastWest, NorthSouth, NorthEast, NorthWest, SouthEast, SouthWest, EastWest, North, South, East, West };
public enum RoomGeneralType { All = 0, Surface, Underground, Tree,  };
[System.Serializable]
public class RoomData
{
    //This is a container class for saving rooms

    //How do i handle the layers? this is one layer of tiles.
    public List<TilemapLayerSaveData> mapLayers = new List<TilemapLayerSaveData>();

    //This array tells us which sides of this room are open.
    // 0 - North
    // 1 - South
    // 2 - East
    // 3 - West
    public bool[] openings = new bool[(int)RoomOpening.Count];
    public RoomGeneralType generalType;

    //It will also need a world type and room type
    // World type - eg. Lava world, Forest World, etc.
    // Room type - Treasure Room, Starting room, etc.
    // maybe another type for where in the map it is? (above or below surface things like that)

    //Maybe a name aswell

    public static RoomData DeepCopy(RoomData original)
    {
        RoomData data = new RoomData();
        data.mapLayers = new List<TilemapLayerSaveData>();

        foreach (TilemapLayerSaveData layer in original.mapLayers)
        {
            data.mapLayers.Add(TilemapLayerSaveData.DeepCopy(layer));
        }

        data.openings = original.openings;

        return data;
    } 

    public RoomAccessType GetRoomAccessType()
    {
        if(openings[0] && openings[1] && openings[2] && openings[3])
        {
            return RoomAccessType.NorthSouthEastWest;
        }
        else if (openings[0] && openings[2] && openings[3])
        {
            return RoomAccessType.NorthEastWest;
        }
        else if (openings[0] && openings[1] && openings[3])
        {
            return RoomAccessType.NorthSouthWest;
        }
        else if (openings[0] && openings[1] && openings[2])
        {
            return RoomAccessType.NorthSouthEast;
        }
        else if (openings[1] && openings[2] && openings[3])
        {
            return RoomAccessType.SouthEastWest;
        }
        else if (openings[0] && openings[1])
        {
            return RoomAccessType.NorthSouth;
        }
        else if (openings[0] && openings[2])
        {
            return RoomAccessType.NorthEast;
        }
        else if (openings[0] && openings[3])
        {
            return RoomAccessType.NorthWest;
        }
        else if (openings[1] && openings[2])
        {
            return RoomAccessType.SouthEast;
        }
        else if (openings[1] && openings[3])
        {
            return RoomAccessType.SouthWest;
        }
        else if (openings[2] && openings[3])
        {
            return RoomAccessType.EastWest;
        }
        else if (openings[0])
        {
            return RoomAccessType.North;
        }
        else if (openings[1])
        {
            return RoomAccessType.South;
        }
        else if (openings[2])
        {
            return RoomAccessType.East;
        }
        else if (openings[3])
        {
            return RoomAccessType.West;
        }

        return RoomAccessType.Closed;
    }
}
