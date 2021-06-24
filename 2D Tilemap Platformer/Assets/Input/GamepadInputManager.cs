using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class GamepadInputManager : MonoBehaviour
{
    public static GamepadInputManager instance;
    PlayerInputManager inputManager;
    public NewGamepadInput[] gamepadInputs = new NewGamepadInput[4];
    public PlayerController playerPrefab;
    public PlayerController[] players;
    public int numActivePlayers = 0;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        inputManager = GetComponent<PlayerInputManager>();
        gamepadInputs = new NewGamepadInput[inputManager.maxPlayerCount];
        players = new PlayerController[inputManager.maxPlayerCount];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        numActivePlayers++;

        Debug.Log("Player " + playerInput.playerIndex+1 + " joined!");
        //input = playerInput;

        //CreationPanelsUI.instance.creationPanels[inputManager.playerCount-1].NewCharacter(playerInput);
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {
        numActivePlayers--;
        Debug.Log("Player " + inputManager.playerCount+1 + " left!");

    }

    public void RemovePlayerAtIndex(int index)
    {
        if(gamepadInputs[index] == null)
        {
            return;
        }
        GameCamera.instance.RemovePlayer(gamepadInputs[index].player);
        //Destroy(gamepadInputs[index].player._input);
        gamepadInputs[index].player._input.SetGamepadInput(null);
        Destroy(gamepadInputs[index].gameObject);
    }

}
