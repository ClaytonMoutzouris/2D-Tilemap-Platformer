using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipDisplay : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
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
