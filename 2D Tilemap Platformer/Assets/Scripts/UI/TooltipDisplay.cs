using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipDisplay : MonoBehaviour
{
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    public void DisplayTooltip(string tooltip)
    {
        text.text = tooltip;
    }

    public void ClearTooltip()
    {
        text.text = "";

    }
}
