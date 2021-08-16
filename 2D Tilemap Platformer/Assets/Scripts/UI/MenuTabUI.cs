using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//This is for main menu tabs, where all players should have control
public class MenuTabUI : MonoBehaviour
{
    public GameObject anchorObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnEnable()
    {
        if (anchorObject == null || GamepadInputManager.instance == null || GamepadInputManager.instance.gamepadInputs == null)
        {
            return;
        }

        foreach(NewGamepadInput input in GamepadInputManager.instance.gamepadInputs)
        {
            if (input != null)
            {
                //GamepadInputManager.instance.gamepadInputs[playerIndex].GetComponent<EventSystem>().SetSelectedGameObject(anchorObject);
                StartCoroutine(UIUtilities.SelectAnchorObject(input.GetComponent<EventSystem>(), anchorObject));
            }
        }

    }

    public GameObject GetAnchorObject()
    {
        return anchorObject;
    }

    public void CloseTab()
    {
        gameObject.SetActive(false);
    }

    public void OpenTab()
    {
        gameObject.SetActive(true);
    }
}
