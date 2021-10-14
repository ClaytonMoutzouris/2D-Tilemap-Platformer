using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ScrollMenuOrientation { Vertical, Horizontal };

public class UIScrollMenu : MonoBehaviour
{
    public GameObject container;
    public ScrollRect scrollRect;
    public MenuOption currentNode;
    public List<MenuOption> nodes = new List<MenuOption>();
    public MenuOption backButton;
    public MenuOption confirmButton;

    public virtual void Start()
    {
        LoadMenuOptions();


    }

    public virtual void LoadMenuOptions()
    {
        AddOption(backButton);
        AddOption(confirmButton);
    }

    public virtual void AddOption(MenuOption option)
    {
        option.SetOption(this, nodes.Count);
        nodes.Add(option);

        //Do this individually
        SetNavigation();
    }

    public virtual void SetCurrentNode(int index)
    {
        SetCurrentNode(nodes[index]);
    }

    public virtual void SetCurrentNode(MenuOption node)
    {
        currentNode = node;
        scrollRect.content.localPosition = scrollRect.GetSnapToPositionToBringChildIntoViewVertical(currentNode.GetComponent<RectTransform>());


    }

    public void SetNav()
    {
        for (int i = 0; i < nodes.Count; i++)
        {



            Navigation customNav = nodes[i].button.navigation;

            if (i == 0)
            {
                //I need to save this for later if i want to loop from top to bottom on options
                //customNav.selectOnUp = interactableUp;
                customNav.selectOnDown = nodes[i + 1].GetComponent<Button>();

            }
            else if (i == nodes.Count - 1)
            {
                customNav.selectOnUp = nodes[i - 1].GetComponent<Button>();
            }
            else
            {
                customNav.selectOnUp = nodes[i - 1].GetComponent<Button>();
                customNav.selectOnDown = nodes[i + 1].GetComponent<Button>();
            }


            nodes[i].GetComponent<Button>().navigation = customNav;

        }
    }

    public void SetNavigation()
    {
        MenuOption first = null;
        MenuOption last = null;
        MenuOption prev = null;

        foreach (MenuOption node in nodes)
        {
            if (!node.gameObject.activeSelf)
            {
                continue;
            }

            Navigation currentNav = node.button.navigation;

            if (prev != null)
            {

                Navigation prevNav = prev.button.navigation;
                prevNav.selectOnDown = node.button;

                currentNav.selectOnUp = prev.button;
                prev.button.navigation = prevNav;

                node.button.navigation = currentNav;

            }



            prev = node;
            last = node;

        }

    }
}
