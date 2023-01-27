using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public ItemObject itemPrefab;

    public Dictionary<EquipmentSlot, Armor> armorEquipment;
    public Dictionary<WeaponSlot, Weapon> weaponSlots;

    int consumableSelectionIndex = 0;
    public int consumableSlots = 5;
    public ConsumableItem[] consumables;

    public PlayerController player;
    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponent<PlayerController>();

        weaponSlots = new Dictionary<WeaponSlot, Weapon>();

        foreach (WeaponSlot slot in System.Enum.GetValues(typeof(WeaponSlot)))
        {
            weaponSlots.Add(slot, null);
        }

        armorEquipment = new Dictionary<EquipmentSlot, Armor>();

        foreach (EquipmentSlot slot in System.Enum.GetValues(typeof(EquipmentSlot)))
        {
            armorEquipment.Add(slot, null);
        }

        consumables = new ConsumableItem[consumableSlots];
    }

    public void AddConsumable(ConsumableItem consumable)
    {
        for(int i = 0; i < consumableSlots; i++)
        {
            //Check for open slots
            if(!consumables[i])
            {
                consumables[i] = consumable;
                player.playerVersusUI.itemBelt.SetItem(consumables[i], i);
                return;
            }
        }

        //ok we have no open slots, we will swap with the currently selected consumable
        if(consumables[consumableSelectionIndex] != null)
        {
            DropItem(consumables[consumableSelectionIndex]);

            consumables[consumableSelectionIndex] = consumable;
            player.playerVersusUI.itemBelt.SetItem(consumables[consumableSelectionIndex], consumableSelectionIndex);
        }
        

    }

    public void UseNextConsumable()
    {

        for (int i = 0; i < consumableSlots; i++)
        {
            //Check for open slots
            if (consumables[i])
            {
                ConsumableItem temp = consumables[i];
                bool used = temp.Use(player);

                if (used)
                {
                    consumables[i] = null;

                    player.playerVersusUI.itemBelt.SetItem(consumables[i], i);
                    

                }
            }
        }

    }

    public bool IsSlotEmpty(WeaponSlot slot)
    {
        return weaponSlots[slot] == null;
    }

    public bool IsSlotEmpty(EquipmentSlot slot)
    {
        return armorEquipment[slot] == null;
    }

    public Armor GetEquipment(EquipmentSlot slot)
    {
        return armorEquipment[slot];
    }

    public Weapon GetEquippedWeapon(WeaponSlot slot)
    {
        return weaponSlots[slot];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipItem(Equipment equipment)
    {

        if(equipment is Weapon weapon)
        {
            if (!IsSlotEmpty(weapon.weaponSlot))
            {
                Unequip(weapon.weaponSlot);
            }

            weaponSlots[weapon.weaponSlot] = weapon;

            weapon.OnEquipped(player);
            if(weapon.weaponSlot == WeaponSlot.Melee)
            {
                player.playerVersusUI.slot1.SetWeapon(weaponSlots[weapon.weaponSlot]);

            }
            else if(weapon.weaponSlot == WeaponSlot.Ranged) {
                player.playerVersusUI.slot2.SetWeapon(weaponSlots[weapon.weaponSlot]);
                player.playerVersusUI.ammoDisplay.UpdateAmmo();

            }

        }
        else if(equipment is Armor armor)
        {
            if(!IsSlotEmpty(armor.equipmentSlot))
            {
                Unequip(armor.equipmentSlot);
            }

            Debug.Log("Equipped item " + armor.name);
            armorEquipment[armor.equipmentSlot] = armor;
            armorEquipment[armor.equipmentSlot].OnEquipped(player);
        }
    }

    public void Unequip(EquipmentSlot slot)
    {
        Armor temp = armorEquipment[slot];
        if(temp != null)
        {
            temp.OnUnequipped(player);
            DropItem(temp);
            armorEquipment[slot] = null;
        }


    }

    public void Unequip(WeaponSlot slot)
    {
        Weapon currentEquipped = weaponSlots[slot];

        if (currentEquipped != null)
        {
            currentEquipped.OnUnequipped(player);

            if (currentEquipped.weaponSlot == WeaponSlot.Melee)
            {
                player.playerVersusUI.slot1.ClearSlot();
            }
            else if (currentEquipped.weaponSlot == WeaponSlot.Ranged)
            {
                player.playerVersusUI.slot2.ClearSlot();

            }

            DropItem(currentEquipped);

        }


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

}
