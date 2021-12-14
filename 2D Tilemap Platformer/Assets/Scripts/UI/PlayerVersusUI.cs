using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVersusUI : MonoBehaviour
{
    public PlayerController player;
    public HealthBarUI healthBar;
    public PlayerTooltip tooltip;
    public Camera playerCamera;
    public Text livesCounter;
    public Text killCounter;
    public PlayerMenu playerMenu;

    public bool showItemTooltips = true;

    // Start is called before the first frame update
    void Start()
    {
        tooltip.versusUI = this;
        tooltip.HideTooltip();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayer(PlayerController player)
    {
        if(this.player != null)
        {
            this.player.health.SetHealthBar(null);
            this.player.playerVersusUI = null;
        }

        this.player = player;
        player.health.SetHealthBar(healthBar);
        player.playerVersusUI = this;
        SetLives();
        SetKills();
    }

    public void SetLives()
    {
        livesCounter.text = "Lives: " + player.playerData.lives;
    }

    public void SetKills()
    {
        killCounter.text = "Kills: " + player.kills;
    }

    public void PauseMenu()
    {

    }
}
