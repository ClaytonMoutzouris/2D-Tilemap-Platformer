using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanelUI : UIScrollMenu
{
    public PlayerMenuTabUI menuTab;
    public MenuOptionInteger optionPrefab;
    //This will be tricky, making the child 
    public int statsToSpend = 5;
    public Text statPointsText;
    public List<Stat> stats = new List<Stat>();

    public void LoadStats()
    {
        stats = new List<Stat>();

        foreach (StatType type in System.Enum.GetValues(typeof(StatType)))
        {
            Stat stat = new Stat(type, 1);
            stats.Add(stat);
        }
    }

    public override void LoadMenuOptions()
    {
        base.LoadMenuOptions();

        foreach (StatType type in System.Enum.GetValues(typeof(StatType)))
        {
            MenuOptionInteger statOption = Instantiate(optionPrefab, container.transform);

            statOption.minValue = 1;
            statOption.SetValue(1);
            statOption.OnValueChanged += OnStatChanged;
            statOption.Init();
            statOption.nameText.text = type.ToString();
            AddOption(statOption);
        }

        SetNavigation();
        SetCurrentNode(0);
        GetComponent<PlayerMenuTabUI>().anchorObject = currentNode.gameObject;
        menuTab.SetAnchor();
    }

    public bool OnStatChanged(int difference)
    {

        if(difference > 0)
        {
            //increased the stat, try to spend the point

            if (statsToSpend - difference >= 0)
            {
                statsToSpend -= difference;
            } else
            {
                //menuOption.SetValue(menuOption.value - difference);
                return false;
            }
        } else if(difference < 0)
        {
            //decreased the stat, refund the point
            statsToSpend += Mathf.Abs(difference);
        }

        statPointsText.text = "Stat Points: " + statsToSpend;
        return true;
    }

    public int GetValueForStat(StatType type)
    {
        MenuOptionInteger node = (MenuOptionInteger)nodes[(int)type + 2];
        return node.GetValue();
    }

    public void ConfirmStats()
    {
        stats = new List<Stat>();

        foreach (StatType type in System.Enum.GetValues(typeof(StatType)))
        {
            Stat stat = new Stat(type, GetValueForStat(type));
            stats.Add(stat);
        }
    }

    public void OnDestroy()
    {
        foreach(MenuOption node in nodes)
        {
            if(node is MenuOptionInteger integerOption)
            {
                integerOption.OnValueChanged -= OnStatChanged;
            }
        }
    }

}
