using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponSlotDisplay : MonoBehaviour
{
    public Image weaponImage;
    public Image background;
    public Sprite emptySprite;

    public void SetWeapon(Weapon wep)
    {
        if(!wep)
        {
            weaponImage.sprite = emptySprite;
            return;
        }

        weaponImage.sprite = wep.sprite;
        weaponImage.color = wep.color;
    }

    public void ClearSlot()
    {
        weaponImage.sprite = emptySprite;
    }
}
