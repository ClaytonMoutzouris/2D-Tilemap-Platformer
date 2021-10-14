using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptionColorSelector : MenuOptionSelector
{

    public MenuColorSelectorNodeUI colorNodePrefab;

    public void AddColorOption(Color color)
    {
        MenuColorSelectorNodeUI newNode = Instantiate(colorNodePrefab, nodesContainer.transform);
        newNode.SetColor(color);
        newNode.parent = this;
        optionNodes.Add(newNode);
    }

    public MenuColorSelectorNodeUI GetColorNode()
    {
        if(currentNode is MenuColorSelectorNodeUI colorNode)
        {
            return colorNode;
        }

        return null;
    }
}
