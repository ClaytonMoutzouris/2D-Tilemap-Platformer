using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Can this just be another GameData or something?
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameData gameData;
    public PlayerController[] players = new PlayerController[4];
    public PlayerController playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        } else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
        gameData = Resources.Load<GameData>("ScriptableObjects/GameData");

    }

    public void RemovePlayerAtIndex(int index)
    {
        if (players[index] == null)
        {
            return;
        }
        GameCamera.instance.RemovePlayer(players[index]);
        //Destroy(gamepadInputs[index].player._input);

        //Player should handle this themselves?
        players[index]._input.SetGamepadInput(null);
        //^^ put this in the player themself

        //Destroy(players[index].gameObject);
        //GamepadInputManager.instance.DropPlayer(index);
        //GamepadInputManager.instance.gamepadInputs[index].
    }

    public void SpawnPlayer(int index, SpawnPoint spawnPoint)
    {
        if (gameData != null && gameData.playerDatas[index] != null)
        {

        }

        PlayerController newPlayer = Instantiate(GamepadInputManager.instance.playerPrefab);
        newPlayer.playerIndex = index;
        newPlayer.transform.position = spawnPoint.transform.position;
        newPlayer._input.SetGamepadInput(GamepadInputManager.instance.gamepadInputs[index]);

        //This really shouldnt be getting done here
        if (gameData != null && gameData.playerDatas[index] != null)
        {
            newPlayer.colorSwap.SetBaseColors(gameData.playerDatas[index].playerColors);

        }
        else
        {
            switch (index)
            {
                case 0:
                    newPlayer.colorSwap.SwapColor(SwapIndex.HoodPrimary, Color.blue);
                    newPlayer.colorSwap.SwapColor(SwapIndex.HoodSecondary, new Color(0.0f, 0.0f, 0.5f, 1));
                    newPlayer.colorSwap.SwapColor(SwapIndex.ShirtPrimary, new Color(0.25f, 0.25f, 0.85f, 1));
                    newPlayer.colorSwap.SwapColor(SwapIndex.Pants, new Color(0.35f, 0.35f, 0.35f, 1));

                    break;
                case 1:
                    newPlayer.colorSwap.SwapColor(SwapIndex.HoodPrimary, Color.red);
                    newPlayer.colorSwap.SwapColor(SwapIndex.HoodSecondary, new Color(0.5f, 0.0f, 0.0f, 1));
                    newPlayer.colorSwap.SwapColor(SwapIndex.ShirtPrimary, new Color(0.85f, 0.25f, 0.25f, 1));
                    newPlayer.colorSwap.SwapColor(SwapIndex.Pants, new Color(0.35f, 0.35f, 0.35f, 1));
                    break;
                case 2:
                    newPlayer.colorSwap.SwapColor(SwapIndex.HoodPrimary, Color.green);
                    newPlayer.colorSwap.SwapColor(SwapIndex.HoodSecondary, new Color(0.0f, 0.5f, 0.0f, 1));
                    newPlayer.colorSwap.SwapColor(SwapIndex.ShirtPrimary, new Color(0.25f, 0.85f, 0.25f, 1));
                    newPlayer.colorSwap.SwapColor(SwapIndex.Pants, new Color(0.35f, 0.35f, 0.35f, 1));
                    break;
                case 3:
                    newPlayer.colorSwap.SwapColor(SwapIndex.HoodPrimary, Color.yellow);
                    newPlayer.colorSwap.SwapColor(SwapIndex.HoodSecondary, new Color(0.7f, 0.7f, 0.0f, 1));
                    newPlayer.colorSwap.SwapColor(SwapIndex.ShirtPrimary, new Color(0.85f, 0.85f, 0.25f, 1));
                    newPlayer.colorSwap.SwapColor(SwapIndex.Pants, new Color(0.35f, 0.35f, 0.35f, 1));
                    break;
            }
        }

        players[newPlayer.playerIndex] = newPlayer;

        GameCamera.instance.AddPlayer(newPlayer);
        //CreationPanelsUI.instance.creationPanels[input.playerIndex].NewCharacter(this);
    }

}
