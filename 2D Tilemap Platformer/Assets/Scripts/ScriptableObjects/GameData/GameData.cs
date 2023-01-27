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
    public int talentPoints = 0;
    public int statPoints = 0;
    public int levelTier = 0;
}

// If you want the data to be stored permanently in the editor
// and e.g. set it via the Inspector
// your types need to be Serializable!
//
// I intentionally used a non-serializable class here to show that also 
// non Serializable types can be passed between scenes 
[System.Serializable]
public class PlayerCreationData
{
    public List<Color> playerColors = new List<Color>();
    public int playerIndex;
    public int lives;
    public List<Talent> talents = new List<Talent>();
    public List<Stat> startingStats = new List<Stat> {
        new Stat(StatType.Attack, 0),
        new Stat(StatType.Defense, 0),
        new Stat(StatType.Constitution, 0),
        new Stat(StatType.Speed, 0),
        new Stat(StatType.Luck, 0)
    };
    public int levelTier;
    public ClassData classData;

}

[System.Serializable]
public class PlayerSaveData
{
    public List<SaveableColor> playerColors = new List<SaveableColor>();
    public List<Talent> talents = new List<Talent>();
    public List<Stat> startingStats = new List<Stat>();
    public int levelTier;

    public PlayerSaveData()
    {

    }

    public PlayerSaveData(PlayerCreationData data)
    {
        //talents = data.talents;
        startingStats = data.startingStats;
        levelTier = data.levelTier;
        playerColors = new List<SaveableColor>();

        foreach (Color c in data.playerColors)
        {
            playerColors.Add(new SaveableColor(c));
        }
    }

    public List<Color> GetColors()
    {
        List<Color> colors = new List<Color>();

        foreach(SaveableColor sColor in playerColors)
        {
            colors.Add(sColor.GetColor());
        }

        return colors;
    }

}

public class SaveableColor
{
    public float[] values = new float[4];

    public SaveableColor(Color color)
    {
        SetColor(color);
    }

    public Color GetColor()
    {
        return new Color(values[0], values[1], values[2], values[3]);
    }

    public void SetColor(Color color)
    {
        values[0] = color.r;
        values[1] = color.g;
        values[2] = color.b;
        values[3] = color.a;
    }
}