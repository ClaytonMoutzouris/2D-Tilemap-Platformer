using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public MultiCamera[] multiCameras = new MultiCamera[4];

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateCameraConfig();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPlayer(PlayerController playerController)
    {
        multiCameras[playerController.playerIndex].AddPlayer(playerController);
        //multiCameras[playerController.playerIndex].gameObject.SetActive(true);
        playerController.playerVersusUI.playerCamera = multiCameras[playerController.playerIndex].mCamera;
        UpdateCameraConfig();
    }

    public void RemovePlayer(PlayerController playerController)
    {
        multiCameras[playerController.playerIndex].RemovePlayer(playerController);
        //multiCameras[playerController.playerIndex].gameObject.SetActive(false);

        UpdateCameraConfig();
    }

    public void ClearCameras()
    {
        foreach(MultiCamera cam in multiCameras)
        {
            if(cam != null)
                cam.gameObject.SetActive(false);
        }

    }

    public void UpdateCameraConfig()
    {
        int numPlayers = 0;

        if(GameManager.instance != null)
        {
            numPlayers = GameManager.instance.NumActivePlayers();
        }

        switch(numPlayers)
        {
            case 0:
            case 1:
                SinglePlayerCamera();
                break;
            case 2:
                TwoPlayerCamera();
                break;
            case 3:
            case 4:
                FourPlayerCamera();
                break;
        }


    
    }

    public List<MultiCamera> GetActiveCameras()
    {
        List<MultiCamera> cameras = new List<MultiCamera>();

        foreach(PlayerController player in GameManager.instance.players)
        {
            if(player != null)
                cameras.Add(multiCameras[player.playerIndex]);
        }


        return cameras;
    }

    

    public void SinglePlayerCamera()
    {

        List<MultiCamera> cameras = GetActiveCameras();
        ClearCameras();
        MultiCamera cam;

        if (cameras.Count == 0)
        {
            //do for player ones cam i guess
            cam = multiCameras[0];
        } else
        {
            cam = cameras[0];
        }

        cam.gameObject.SetActive(true);
        cam.mCamera.rect = new Rect(0, 0, 1, 1);

    }

    public void TwoPlayerCamera()
    {

        List<MultiCamera> cameras = GetActiveCameras();
        ClearCameras();
        MultiCamera camOne = cameras[0];
        MultiCamera camTwo = cameras[1];





        camOne.gameObject.SetActive(true);
        camOne.mCamera.rect = new Rect(0, 0, .5f, 1);

        camTwo.gameObject.SetActive(true);
        camTwo.mCamera.rect = new Rect(.5f, 0, .5f, 1);

    }

    public void FourPlayerCamera()
    {
    
        multiCameras[0].gameObject.SetActive(true);
        multiCameras[0].mCamera.rect = new Rect(0, .5f, .5f, .5f);

        multiCameras[1].gameObject.SetActive(true);
        multiCameras[1].mCamera.rect = new Rect(.5f, .5f, .5f, .5f);

        multiCameras[2].gameObject.SetActive(true);
        multiCameras[2].mCamera.rect = new Rect(0, 0, .5f, .5f);

        multiCameras[3].gameObject.SetActive(true);
        multiCameras[3].mCamera.rect = new Rect(.5f, 0, .5f, .5f);


    }
}
