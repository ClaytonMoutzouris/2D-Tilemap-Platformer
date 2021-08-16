using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum AppearanceMenuOption { SkinColor, HoodPrimaryColor, HoodSecondaryColor, ShirtPrimaryColor, ShirtSecondaryColor, ShoesColors, PantsColor }
public class AppearancePanelUI : MonoBehaviour
{

    public AppearanceMenuOptions appearanceMenuOptions;
    public List<MenuOptionSelectorUI> menuOptions;
    public CharacterSelectPortrait portrait;
    // Start is called before the first frame update
    void Start()
    {
        LoadMenuOptions();
    }

    public void Update()
    {
        portrait.colorSwap.SetBaseColors(GetColors());

    }

    public List<Color> GetColors()
    {
        List<Color> colors = new List<Color>();
        colors.Add(((MenuColorSelectorNodeUI)menuOptions[(int)AppearanceMenuOption.SkinColor].currentNode).color);
        colors.Add(((MenuColorSelectorNodeUI)menuOptions[(int)AppearanceMenuOption.HoodPrimaryColor].currentNode).color);
        colors.Add(((MenuColorSelectorNodeUI)menuOptions[(int)AppearanceMenuOption.HoodSecondaryColor].currentNode).color);
        colors.Add(((MenuColorSelectorNodeUI)menuOptions[(int)AppearanceMenuOption.ShirtPrimaryColor].currentNode).color);
        colors.Add(((MenuColorSelectorNodeUI)menuOptions[(int)AppearanceMenuOption.ShirtSecondaryColor].currentNode).color);
        colors.Add(((MenuColorSelectorNodeUI)menuOptions[(int)AppearanceMenuOption.PantsColor].currentNode).color);
        colors.Add(((MenuColorSelectorNodeUI)menuOptions[(int)AppearanceMenuOption.ShoesColors].currentNode).color);

        Debug.Log("Colors " + colors);

        foreach (Color c in colors)
        {
            Debug.Log(c.ToString());
        }

        return colors;

    }

    public void LoadMenuOptions()
    {
        if (appearanceMenuOptions == null)
        {
            //this should never happen
            return;
        }

        foreach (Color color in appearanceMenuOptions.skinColors)
        {
            menuOptions[(int)AppearanceMenuOption.SkinColor].AddColorOption(color);
        }
        menuOptions[(int)AppearanceMenuOption.SkinColor].Init();

        foreach (Color color in appearanceMenuOptions.hoodPrimaryColors)
        {
            menuOptions[(int)AppearanceMenuOption.HoodPrimaryColor].AddColorOption(color);
        }
        menuOptions[(int)AppearanceMenuOption.HoodPrimaryColor].Init();

        foreach (Color color in appearanceMenuOptions.hoodSecondaryColors)
        {
            menuOptions[(int)AppearanceMenuOption.HoodSecondaryColor].AddColorOption(color);
        }
        menuOptions[(int)AppearanceMenuOption.HoodSecondaryColor].Init();

        foreach (Color color in appearanceMenuOptions.shirtPrimaryColors)
        {
            menuOptions[(int)AppearanceMenuOption.ShirtPrimaryColor].AddColorOption(color);
        }
        menuOptions[(int)AppearanceMenuOption.ShirtPrimaryColor].Init();

        foreach (Color color in appearanceMenuOptions.shirtSecondaryColors)
        {
            menuOptions[(int)AppearanceMenuOption.ShirtSecondaryColor].AddColorOption(color);
        }
        menuOptions[(int)AppearanceMenuOption.ShirtSecondaryColor].Init();

        foreach (Color color in appearanceMenuOptions.shoesColors)
        {
            menuOptions[(int)AppearanceMenuOption.ShoesColors].AddColorOption(color);
        }
        menuOptions[(int)AppearanceMenuOption.ShoesColors].Init();

        foreach (Color color in appearanceMenuOptions.pantsColors)
        {
            menuOptions[(int)AppearanceMenuOption.PantsColor].AddColorOption(color);
        }
        menuOptions[(int)AppearanceMenuOption.PantsColor].Init();



    }

    public void SetNavigation()
    {
        for (int i = 0; i < menuOptions.Count; i++)
        {

            Navigation customNav = menuOptions[i].button.navigation;

            if (i == 0)
            {
                //I need to save this for later if i want to loop from top to bottom on options
                //customNav.selectOnUp = interactableUp;
                customNav.selectOnDown = menuOptions[i + 1].GetComponent<Button>();

            }
            else if (i == menuOptions.Count - 1)
            {
                customNav.selectOnUp = menuOptions[i - 1].GetComponent<Button>();
            }
            else
            {
                customNav.selectOnUp = menuOptions[i - 1].GetComponent<Button>();
                customNav.selectOnDown = menuOptions[i + 1].GetComponent<Button>();
            }


            menuOptions[i].GetComponent<Button>().navigation = customNav;

        }
    }
}
