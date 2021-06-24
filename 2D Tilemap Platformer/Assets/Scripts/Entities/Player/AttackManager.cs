using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    //List of different player attack animations
    public PlayerController entity;
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
        entity = GetComponent<PlayerController>();
        List<Weapon> tempList = new List<Weapon>();

        //I Set these in the inspector for now
        /*
        rangedWeaponObject.SetOwner(entity);
        meleeWeaponObject.SetOwner(entity);
        */

        foreach (Weapon weapon in weapons)
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

    public void ActivateAttack()
    {
        if (activeAttack != null)
        {
            return;
        }

        Attack newAttack = equippedWeapon.GetNextAttack();
        if (newAttack == null)
        {
            return;
        }
        StartCoroutine(newAttack.Activate(entity));
        activeAttack = newAttack;

    }

    public void ActivateHeavyAttack()
    {
        if (activeAttack != null)
        {
            return;
        }

        Attack newAttack = equippedWeapon.GetHeavyAttack();
        if(newAttack == null)
        {
            return;
        }
        StartCoroutine(newAttack.Activate(entity));
        activeAttack = newAttack;
    }

    public void FireProjectile()
    {
        Projectile proj = equippedWeapon.projectile;

        if(proj != null)
        {
            proj = Instantiate(proj, entity.transform.position, Quaternion.identity);
            proj._attackObject.SetOwner(entity);
            proj._attackObject.damage = equippedWeapon.damage;
            proj._attackObject.knockbackPower = equippedWeapon.knockbackPower;
            proj.SetDirection(entity.GetDirection() * Vector2.right);
        }


    }

}
