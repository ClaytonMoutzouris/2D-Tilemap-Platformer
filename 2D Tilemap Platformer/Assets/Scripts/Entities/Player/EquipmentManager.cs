﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public ItemObject itemPrefab;

    public Weapon equippedWeapon;
    public Weapon storedWeapon;

    public Dictionary<EquipmentSlot, Armor> armorEquipment;
    public List<ConsumableItem> consumables;

    public PlayerController player;
    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponent<PlayerController>();
        armorEquipment = new Dictionary<EquipmentSlot, Armor>();

        foreach (EquipmentSlot slot in System.Enum.GetValues(typeof(EquipmentSlot)))
        {
            armorEquipment.Add(slot, null);
        }

        consumables = new List<ConsumableItem>();
    }

    public void AddConsumable(ConsumableItem consumable)
    {
        consumables.Add(consumable);
        player.playerVersusUI.consumableSlot.SetItem(consumables[0]);

    }

    public void UseNextConsumable()
    {
        if(consumables.Count <= 0)
        {
            return;
        }

        ConsumableItem temp = consumables[0];
        bool used = temp.Use(player);

        if(used)
        {
            consumables.RemoveAt(0);
            if (consumables.Count > 0)
            {
                player.playerVersusUI.consumableSlot.SetItem(consumables[0]);
            } else
            {
                player.playerVersusUI.consumableSlot.ClearSlot();
            }

        }

    }

    public bool IsSlotEmpty(EquipmentSlot slot)
    {
        return armorEquipment[slot] == null;
    }

    public Armor GetEquipment(EquipmentSlot slot)
    {
        return armorEquipment[slot];
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
                DropItem(equippedWeapon);

            }

            equippedWeapon = weapon;

            weapon.OnEquipped(player);
            player.playerVersusUI.slot1.SetWeapon(equippedWeapon);

        }
        else if(equipment is Armor armor)
        {
            if(!IsSlotEmpty(armor.equipmentSlot))
            {
                UnequipItem(armor.equipmentSlot);
            }

            Debug.Log("Equipped item " + armor.name);
            armorEquipment[armor.equipmentSlot] = armor;
            armorEquipment[armor.equipmentSlot].OnEquipped(player);
        }
    }

    public void UnequipItem(EquipmentSlot slot)
    {
        Armor temp = armorEquipment[slot];
        if(temp != null)
        {
            temp.OnUnequipped(player);
            DropItem(temp);
            armorEquipment[slot] = null;
        }


    }

    public void EquipWeapon(Weapon wep)
    {
        if (equippedWeapon != null)
        {
            equippedWeapon.OnUnequipped(player);
            DropItem(equippedWeapon);

        }

        equippedWeapon = wep;
        player.playerVersusUI.slot1.SetWeapon(equippedWeapon);
        wep.OnEquipped(player);
    }

    public void DropItem(ItemData item)
    {

        Vector2 dir = Random.Range(-1f, 1f) * Vector2.right + Vector2.up;

        ItemObject dropped = Instantiate(itemPrefab);
        dropped.transform.position = player.transform.position;
        dropped.SetItem(item);

        /*
        dropped._velocity.y = Mathf.Sqrt(4 * dir.normalized.y * -GambleConstants.GRAVITY);
        //Debug.Log("Velocity " + _velocity);
        _velocity.x = direction.normalized.x * projectileData.projSpeed;
        */

        dropped._controller.velocity = dir * 4;
        dropped.StartCoroutine(dropped.Despawn(5));


    }

    public void SwapWeaponSlots()
    {
        Weapon toEquip = storedWeapon;
        if(equippedWeapon != null)
        {
            equippedWeapon.OnUnequipped(player);
            storedWeapon = equippedWeapon;
            equippedWeapon = null;

        }

        if(toEquip != null)
        {
            toEquip.OnEquipped(player);
            equippedWeapon = toEquip;

        }

        player.playerVersusUI.slot2.SetWeapon(storedWeapon);
        player.playerVersusUI.slot1.SetWeapon(equippedWeapon);


    }
}
