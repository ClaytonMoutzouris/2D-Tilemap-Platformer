using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptionInteger : MenuOption
{
    public TextMeshProUGUI valueText;
    public int value = 1;
    public int tempValue = 1;
    public Button decButton;
    public Button incButton;

    public int minValue = 0;
    public int maxValue = 99;

    public delegate bool ValueChangedEvent(int difference);

    public event ValueChangedEvent OnValueChanged;

    public void Start()
    {
        SetValue(value);
    }

    public void SetValue(int val)
    {
        int prevVal = value;
        value = Mathf.Clamp(val, minValue, maxValue);

        if (OnValueChanged != null && !OnValueChanged(value - prevVal))
        {
            value = prevVal;    
        }


        valueText.text = value.ToString();

    }

    public void DecrementValue()
    {
        SetValue(value-1);

    }

    public void IncrementValue()
    {
        SetValue(value+1);
    }

    public int GetValue()
    {
        return value;
    }

    public void RedirectClick()
    {
        decButton.Select();
    }
}
