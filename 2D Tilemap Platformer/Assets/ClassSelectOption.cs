using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClassSelectOption : MenuOption, IDeselectHandler
{
    public ClassData classData;
    public ClassSelectPanelUI menuParent;

    public void SetClass(ClassData data)
    {
        classData = data;
        text.text = classData.classType.ToString();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        SoundManager.instance.PlaySingle(selectSound);
        menuParent.characterScreen.tooltip.text.text = classData.GetTooltip();
        parent.SetCurrentNode(this);

    }

    public void OnDeselect(BaseEventData eventData)
    {
        menuParent.characterScreen.tooltip.text.text = "";

    }

    public void ChooseClass()
    {
        menuParent.SelectClass(classData);
    }
}
