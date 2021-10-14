using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVersusUIController : MonoBehaviour
{
    public static PlayerVersusUIController instance;
    public PlayerVersusUI[] uiPanels = new PlayerVersusUI[4];

    // Start is called before the first frame update
    void Start()
    {
        instance = this;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayer(int playerIndex, PlayerController player)
    {
        uiPanels[playerIndex].SetPlayer(player);
    }
}
