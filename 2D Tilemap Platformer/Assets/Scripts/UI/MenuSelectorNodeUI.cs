using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSelectorNodeUI : MonoBehaviour, ISelectHandler
{
    public Button button;
    public MenuOptionSelector parent;
    public TextMeshProUGUI text;
    public AudioClip selectClip;

    public void Start()
    {
        //parent = GetComponentInParent<MenuOptionUI>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        parent.SetCurrentNode(this);
    }

    public void SelectOption()
    {
        SoundManager.instance.PlaySingle(selectClip);
        parent.SelectNode(this);
    }

    public void SetNavigation()
    {

    }

    public void Confirm()
    {
        if(parent == null)
        {
            return;
        }
        parent.button.Select();
    }

}
