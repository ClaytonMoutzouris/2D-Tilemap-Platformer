using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public static class RoomDatabase
{
    public static List<RoomData> rooms = new List<RoomData>();
    public static bool reload = true;

    static void CheckDatabase()
    {
        if(reload)
        {
            LoadRooms();
            reload = false;
        }
    }

    //Loads rooms into memory for parsing
    public static void LoadRooms()
    {

        foreach (string room in Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "GameData", "Rooms"), "*.room"))
        {
            if (File.Exists(room))
            {
                string loadJson = File.ReadAllText(room);

                RoomData loadData = JsonConvert.DeserializeObject<RoomData>(loadJson);
                rooms.Add(loadData);
            }
            else
            {
                Debug.LogError("Room file not found: " + room);
            }
        }
    }

    public static RoomData GetRoom(RoomAccessType type)
    {
        CheckDatabase();
        List<RoomData> eligibleRooms = new List<RoomData>();
        foreach(RoomData data in rooms)
        {
            if(data.GetRoomAccessType() == type)
            {
                eligibleRooms.Add(data);
            }
        }

        if(eligibleRooms.Count > 0)
        {
            return RoomData.DeepCopy(eligibleRooms[Random.Range(0, eligibleRooms.Count)]);
        } else
        {
            return new RoomData();
        }
    }

    public static RoomData GetRoom(RoomAccessType access, RoomGeneralType generalType)
    {
        CheckDatabase();
        List<RoomData> eligibleRooms = new List<RoomData>();
        foreach (RoomData data in rooms)
        {
            if (data.GetRoomAccessType() == access && (data.generalType == RoomGeneralType.All || data.generalType == generalType))
            {
                eligibleRooms.Add(data);
            }
        }

        if (eligibleRooms.Count > 0)
        {
            return RoomData.DeepCopy(eligibleRooms[Random.Range(0, eligibleRooms.Count)]);
        }
        else
        {
            //else its a blank room for now
            return new RoomData();
        }
    }

    public static RoomData GetRandomRoom()
    {
        CheckDatabase();
        
        if(rooms.Count > 0)
        {
            return RoomData.DeepCopy(rooms[Random.Range(0, rooms.Count)]);
        }

        return null;
    }
}
