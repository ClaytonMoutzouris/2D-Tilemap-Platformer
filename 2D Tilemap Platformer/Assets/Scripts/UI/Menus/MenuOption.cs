﻿using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuOption : MonoBehaviour, ISelectHandler
{
    public Button button;
    public AudioClip selectSound;
    public UIScrollMenu parent;
    public int optionIndex;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Init()
    {

    }

    public void SetOption(UIScrollMenu menu, int index)
    {
        this.parent = menu;
        optionIndex = index;
    }

    public void SetOptionName(string name)
    {
        text.text = name;
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        SoundManager.instance.PlaySingle(selectSound);
        parent.SetCurrentNode(this);
    }
}

public struct OptionValue {
    public int nodeID;
    public int nodeValue;
}
