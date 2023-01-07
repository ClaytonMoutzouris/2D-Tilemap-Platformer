using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTooltip : MonoBehaviour
{
    public PlayerVersusUI versusUI;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTooltip(ItemObject item)
    {
        gameObject.SetActive(true);
        text.text = item.item.GetTooltip();

        RectTransform rect = GetComponent<RectTransform>();
        RectTransform canvasRect = versusUI.GetComponent<RectTransform>();

        Vector2 adjustedPosition = versusUI.playerCamera.WorldToScreenPoint(item.transform.position);

        
        adjustedPosition.x *= canvasRect.rect.width / (float)versusUI.playerCamera.pixelWidth;
        adjustedPosition.y *= canvasRect.rect.height / (float)versusUI.playerCamera.pixelHeight;
        rect.anchoredPosition = adjustedPosition - canvasRect.sizeDelta / 2f;
        

        //rect.anchoredPosition = adjustedPosition;

    }

    public void HideTooltip()
    {
        text.text = "";
        gameObject.SetActive(false);
    }
}
