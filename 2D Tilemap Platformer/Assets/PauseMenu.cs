using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    public MenuTabUI menuTab;
    public bool gamePaused = false;

    private void Start()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void PauseGame()
    {
        gameObject.SetActive(true);
        gamePaused = true;
        Time.timeScale = 0;

    }

    public void ResumeGame()
    {
        //GameManager.instance.StartGame();
        gamePaused = false;
        gameObject.SetActive(false);
        Time.timeScale = 1;

    }

    public void QuitGame()
    {
        if(gamePaused)
        {
            ResumeGame();
        }

        GameManager.instance.MainMenu();
    }

}
