using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour
{
    public static ScoreScreen instance;
    public MenuTabUI menuTab;
    public TextMeshProUGUI victorText;

    private void Start()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void ShowScreen(PlayerController winner)
    {
        gameObject.SetActive(true);
        victorText.text = "Player " + winner.playerIndex + " Wins!";
    }

    public void PlayAgain()
    {
        GameManager.instance.StartGame();
    }

    public void ReturnToMainMenu()
    {
        GameManager.instance.MainMenu();
    }

}
