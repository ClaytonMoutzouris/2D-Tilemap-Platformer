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
    public int numActivePlayers = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        } else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);

        inputManager = GetComponent<PlayerInputManager>();
        gamepadInputs = new NewGamepadInput[inputManager.maxPlayerCount];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        numActivePlayers++;

        Debug.Log("Player " + (playerInput.playerIndex+1) + " joined!");
        //input = playerInput;
        //playerInput.GetComponent<EventSystem>().SetSelectedGameObject(MainMenu.instance.anchorObject);
        gamepadInputs[playerInput.playerIndex] = playerInput.GetComponent<NewGamepadInput>();

        //Only do this if we're in the main menu, otherwise we'll need to do something else
        if (MainMenu.instance != null)
        {
            MainMenu.instance.NewPlayerJoined(playerInput);
        }


        //CreationPanelsUI.instance.creationPanels[inputManager.playerCount-1].NewCharacter(playerInput);
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {
        numActivePlayers--;
        Debug.Log("Player " + inputManager.playerCount+1 + " left!");

    }

    /**
     * This is for dropping an active player input
     */
    public void DropPlayer(int index)
    {
        if (gamepadInputs[index] == null)
        {
            return;
        }

        Destroy(gamepadInputs[index].gameObject);
    }

    /*
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
    */

}
