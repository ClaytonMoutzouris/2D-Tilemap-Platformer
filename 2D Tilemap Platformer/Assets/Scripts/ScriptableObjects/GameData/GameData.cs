using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// fileName is the default name when creating a new Instance
// menuName is where to find it in the context menu of Create
[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData/GameData")]
public class GameData : ScriptableObject
{
    public GameMode gameMode;
    public string mapName;
    public PlayerCreationData[] playerDatas = new PlayerCreationData[4];
    public int timeLimit = 5;
    public int lives = 1;
}

// If you want the data to be stored permanently in the editor
// and e.g. set it via the Inspector
// your types need to be Serializable!
//
// I intentionally used a non-serializable class here to show that also 
// non Serializable types can be passed between scenes 
public class PlayerCreationData
{
    public List<Color> playerColors = new List<Color>();
    public int playerIndex;
    public int lives;
    public List<Talent> talents;
    public List<Stat> startingStats;

}