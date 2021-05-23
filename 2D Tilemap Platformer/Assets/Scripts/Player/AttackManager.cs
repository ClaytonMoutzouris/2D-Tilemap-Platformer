using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    //List of different player attack animations
    public Entity entity;
    //public List<AttackData> attacks = new List<AttackData>();
    public Attack activeAttack;

    //Where do you really need to be?
    public WeaponObject meleeWeaponObject;
    public WeaponObject rangedWeaponObject;

    //Right now its not, but this should follow up off the end of an attack, like a cooldown.
    public float followUpThreshold = 1f;
    public float lastAttackTime = 0;

    public List<Weapon> weapons;
    public int weaponIndex = 0;
    public Weapon equippedWeapon;


    private void Start()
    {
        entity = GetComponent<Entity>();
        List<Weapon> tempList = new List<Weapon>();

        foreach(Weapon weapon in weapons)
        {
            Weapon temp = Instantiate(weapon);
            tempList.Add(temp);
        }

        weapons = tempList;
        SwapWeapon(weaponIndex);
    }

    public void SetWeapon(Weapon wep)
    {
        if(wep is RangedWeapon)
        {
            rangedWeaponObject.SetWeapon(wep);
        } else
        {
            meleeWeaponObject.SetWeapon(wep);
        }

        equippedWeapon = wep;
    }

    public void SwapWeaponUp()
    {
        weaponIndex++;

        if(weaponIndex >= weapons.Count)
        {
            weaponIndex = 0;
        }

        SwapWeapon(weaponIndex);
    }

    public void SwapWeapon(int index)
    {
        if(index > weapons.Count)
        {
            return;
        }
        weaponIndex = index;

        SetWeapon(weapons[index]);

        //wepRenderer.SetWeapon(equippedWeapon);
        /*
        AttackObject attackObject = equippedWeapon.weaponObject.GetComponent<AttackObject>();
        if (attackObject)
            equippedWeapon.attacks[0].SetObject(attackObject);
        */
    }

    public bool IsAttacking()
    {

            if(activeAttack != null)
            {
                return true;
            }
        

        return false;
    }

    /*
    public void ActivateAttack(int index)
    {
        if(activeAttack != null)
        {
            return;
        }

        Attack newAttack = Instantiate(attacks[index].attack, transform);
        StartCoroutine(newAttack.Activate(player));
        activeAttack = newAttack;
    }
    */

    public void ActivateWeaponAttack()
    {
        if (activeAttack != null)
        {
            return;
        }

        WeaponAttack newAttack = Instantiate(equippedWeapon.GetNextAttack(), transform);
        StartCoroutine(newAttack.Activate(entity));
        activeAttack = newAttack;
        lastAttackTime = Time.time;

    }

}
