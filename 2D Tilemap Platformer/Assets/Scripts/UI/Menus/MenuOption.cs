using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuOption : MonoBehaviour, ISelectHandler
{
    public Button button;
    public AudioClip selectSound;
    public UIScrollMenu parent;
    public int optionIndex;

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

    public virtual void OnSelect(BaseEventData eventData)
    {
        SoundManager.instance.PlaySingle(selectSound);
        parent.SetCurrentNode(this);
    }
}
