using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRenderer : MonoBehaviour
{
    public WeaponObject weapon;

    public void SetWeapon(WeaponObject newWeapon)
    {
        if(weapon != null)
        {
            Destroy(weapon.gameObject);
        }

        weapon = Instantiate(newWeapon, transform);
        //weapon.gameObject.SetActive(false);
    }
}
