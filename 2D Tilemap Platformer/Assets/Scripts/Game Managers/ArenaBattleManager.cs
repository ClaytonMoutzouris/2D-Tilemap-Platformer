using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ArenaBattleManager : MonoBehaviour
{
    public static ArenaBattleManager instance;
    public List<SpawnPoint> spawnPoints;
    public GameData gameData;

    public float gameTimer;
    public Text gameTimeText;

    // Start is called before the first frame update
    void Start()
    {
        //Set the players lives from the gamedata
        instance = this;
        //set the timer from the gamedata


        StartGame();
    }

    public void StartGame()
    {
        StartCoroutine(GameSetup());
    }

    public IEnumerator GameSetup()
    {
        GameManager.instance.ClearPlayers();

        gameTimer = gameData.timeLimit * 60;

        string path = Path.Combine(Application.streamingAssetsPath, "GameData", "Maps", gameData.mapName);
        path += ".map";
        GameGrid.instance.LoadMap(path);

        Debug.Log("Game Starting in 3");
        yield return new WaitForSeconds(1);

        Debug.Log("Game Starting in 2");
        yield return new WaitForSeconds(1);

        Debug.Log("Game Starting in 1");
        yield return new WaitForSeconds(1);

        Debug.Log("Game Start!");

        List<SpawnPoint> pointsList = new List<SpawnPoint>();
        pointsList.AddRange(spawnPoints);

        foreach (PlayerCreationData data in gameData.playerDatas)
        {
            if (data == null)
            {
                continue;
            }
            data.lives = gameData.lives;
            int r = Random.Range(0, pointsList.Count);
            GameManager.instance.SpawnPlayer(data.playerIndex, pointsList[r], data);
            pointsList.RemoveAt(r);
        }

    }

    // Update is called once per frame
    void Update()
    {
        gameTimer -= Time.deltaTime;

        if(gameTimer < 0)
        {
            gameTimer = 0;
        }

        int minutes = Mathf.FloorToInt(gameTimer / 60F);
        int seconds = Mathf.FloorToInt(gameTimer - minutes * 60);
        gameTimeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);


        if(gameTimer <= 0)
        {
            //Game ends
        }

    }

    public IEnumerator RespawnPlayer(int playerIndex, float spawntime = 5)
    {

        yield return new WaitForSeconds(spawntime);

        int r = Random.Range(0, spawnPoints.Count);

        GameManager.instance.SpawnPlayer(playerIndex, spawnPoints[r], gameData.playerDatas[playerIndex]);

    }

}
