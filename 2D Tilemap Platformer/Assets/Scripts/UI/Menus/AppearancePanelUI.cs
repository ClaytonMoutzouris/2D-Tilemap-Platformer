using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum AppearanceMenuOption { Back, Confirm, SkinColor, HoodPrimaryColor, HoodSecondaryColor, ShirtPrimaryColor, ShirtSecondaryColor, ShoesColors, PantsColor }
public class AppearancePanelUI : UIScrollMenu
{

    public AppearanceMenuOptions appearanceMenuOptions;
    public MenuOptionSelector prefab;
    public PlayerMenuTabUI menuTab;
    public List<Color> colors = new List<Color>();
    //public List<MenuOptionSelector> menuOptions;
    // Start is called before the first frame update

    public CharacterSelectScreen characterScreen;

    public void Update()
    {

    }

    public MenuOptionSelector GetNode(int index)
    {
        return (MenuOptionSelector)nodes[index];
    }

    public void LoadColors()
    {
        colors = new List<Color>();

        colors.Add(appearanceMenuOptions.skinColors[0]);
        colors.Add(appearanceMenuOptions.hoodPrimaryColors[0]);
        colors.Add(appearanceMenuOptions.hoodSecondaryColors[0]);
        colors.Add(appearanceMenuOptions.shirtPrimaryColors[0]);
        colors.Add(appearanceMenuOptions.shirtSecondaryColors[0]);
        colors.Add(appearanceMenuOptions.shoesColors[0]);
        colors.Add(appearanceMenuOptions.pantsColors[0]);

        characterScreen.portrait.colorSwap.SetBaseColors(colors);

    }

    public override void SetCurrentNode(MenuOption node)
    {
        base.SetCurrentNode(node);

        if(node is MenuOptionSelector colorSelector)
        {
            //This hard coding is because the back and confirm buttons are there
            colors[node.optionIndex-2] = colorSelector.GetColorNode().color;
            characterScreen.portrait.colorSwap.SetBaseColors(colors);
        }
    }

    public override void LoadMenuOptions()
    {

        base.LoadMenuOptions();

        if (appearanceMenuOptions == null)
        {
            //this should never happen
            return;
        }

        MenuOptionSelector skinColorSelector = Instantiate(prefab, container.transform);
        foreach (Color color in appearanceMenuOptions.skinColors)
        {
            skinColorSelector.AddColorOption(color);
        }
        skinColorSelector.Init();
        skinColorSelector.SetName("Skin");
        AddOption(skinColorSelector);

        MenuOptionSelector hoodPrimaryColorSelector = Instantiate(prefab, container.transform);
        foreach (Color color in appearanceMenuOptions.hoodPrimaryColors)
        {
            hoodPrimaryColorSelector.AddColorOption(color);
        }
        hoodPrimaryColorSelector.Init();
        hoodPrimaryColorSelector.SetName("Hood 1");
        AddOption(hoodPrimaryColorSelector);

        MenuOptionSelector hoodSecondaryColorSelector = Instantiate(prefab, container.transform);
        foreach (Color color in appearanceMenuOptions.hoodSecondaryColors)
        {
            hoodSecondaryColorSelector.AddColorOption(color);
        }
        hoodSecondaryColorSelector.Init();
        hoodSecondaryColorSelector.SetName("Hood 2");

        AddOption(hoodSecondaryColorSelector);

        MenuOptionSelector shirtPrimaryColorSelector = Instantiate(prefab, container.transform);
        foreach (Color color in appearanceMenuOptions.shirtPrimaryColors)
        {
            shirtPrimaryColorSelector.AddColorOption(color);
        }
        shirtPrimaryColorSelector.Init();
        shirtPrimaryColorSelector.SetName("Shirt 1");

        AddOption(shirtPrimaryColorSelector);

        MenuOptionSelector shirtSecondaryColorSelector = Instantiate(prefab, container.transform);
        foreach (Color color in appearanceMenuOptions.shirtSecondaryColors)
        {
            shirtSecondaryColorSelector.AddColorOption(color);
        }
        shirtSecondaryColorSelector.Init();
        shirtSecondaryColorSelector.SetName("Shirt 2");

        AddOption(shirtSecondaryColorSelector);

        MenuOptionSelector shoesColorSelector = Instantiate(prefab, container.transform);
        foreach (Color color in appearanceMenuOptions.shoesColors)
        {
            shoesColorSelector.AddColorOption(color);
        }
        shoesColorSelector.Init();
        shoesColorSelector.SetName("Shoes");

        AddOption(shoesColorSelector);

        MenuOptionSelector pantsColorsSelector = Instantiate(prefab, container.transform);
        foreach (Color color in appearanceMenuOptions.pantsColors)
        {
            pantsColorsSelector.AddColorOption(color);
        }
        pantsColorsSelector.Init();
        pantsColorsSelector.SetName("Pants");

        AddOption(pantsColorsSelector);

        SetNavigation();
        SetCurrentNode(0);
        GetComponent<PlayerMenuTabUI>().anchorObject = currentNode.gameObject;
        menuTab.SetAnchor();

        LoadColors();
    }


}
