using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuColorSelectorNodeUI : MenuSelectorNodeUI
{
    public Image image;
    public Color color;

    public void SetColor(Color color)
    {
        this.color = color;
        image.color = color;
    }
}
