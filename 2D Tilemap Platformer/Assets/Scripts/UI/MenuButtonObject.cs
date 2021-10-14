using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonObject : MonoBehaviour, ISelectHandler
{
    public Button button;
    public AudioClip selectSound;
    public AudioClip confirmSound;

    public void OnSelect(BaseEventData eventData)
    {
        SoundManager.instance.PlaySingle(selectSound);
    }


}
