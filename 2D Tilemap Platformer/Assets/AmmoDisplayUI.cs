using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;

public class AmmoDisplayUI : MonoBehaviour
{
    public PlayerVersusUI versusUI;
    public Image image;
    public TextMeshProUGUI ammoCount;

    public Sprite[] ammoSprites = new Sprite[(int)AmmoType.None+1];

    public void UpdateAmmo()
    {
        //Ammunition ammo = versusUI.player._equipmentManager.GetEquippedWeapon(WeaponSlot.Ranged).
        switch (versusUI.player._equipmentManager.GetEquippedWeapon(WeaponSlot.Ranged).ammoType)
        {
            case AmmoType.None:
                image.sprite = ammoSprites[(int)AmmoType.None];
                ammoCount.text = "~";
                break;
            case AmmoType.Bullets:
                image.sprite = ammoSprites[(int)AmmoType.Bullets];
                ammoCount.text = "x" + versusUI.player._equipmentManager.GetEquippedWeapon(WeaponSlot.Ranged).currentAmmo;

                break;
            case AmmoType.Fuel:
                image.sprite = ammoSprites[(int)AmmoType.Fuel];
                ammoCount.text = "x" + versusUI.player._equipmentManager.GetEquippedWeapon(WeaponSlot.Ranged).currentAmmo;

                break;
            case AmmoType.Plasma:
                image.sprite = ammoSprites[(int)AmmoType.Plasma];
                ammoCount.text = "x" + versusUI.player._equipmentManager.GetEquippedWeapon(WeaponSlot.Ranged).currentAmmo;

                break;
            case AmmoType.Bombs:
                image.sprite = ammoSprites[(int)AmmoType.Bombs];
                ammoCount.text = "x" + versusUI.player._equipmentManager.GetEquippedWeapon(WeaponSlot.Ranged).currentAmmo;

                break;
        }


    }
}
