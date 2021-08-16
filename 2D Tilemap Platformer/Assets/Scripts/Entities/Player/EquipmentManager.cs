using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{

    public Weapon equippedWeapon;
    public Weapon rangedWeapon;

    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipItem(Equipment equipment)
    {

        if(equipment is Weapon weapon)
        {
            if(equippedWeapon != null)
            {
                equippedWeapon.OnUnequipped(player);
            }

            equippedWeapon = weapon;

            weapon.OnEquipped(player);
        }
    }
}
