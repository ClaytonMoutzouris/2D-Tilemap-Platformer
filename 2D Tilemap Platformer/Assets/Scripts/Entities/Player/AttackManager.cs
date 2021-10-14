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



    private void Start()
    {
        entity = GetComponent<PlayerController>();
        List<Weapon> tempList = new List<Weapon>();

    }

    public void SetWeaponObject(Weapon wep)
    {
        if(wep is RangedWeapon)
        {
            rangedWeaponObject.SetWeapon(wep);
        } else
        {
            meleeWeaponObject.SetWeapon(wep);
        }
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

    public void ActivateAttack(ButtonInput button = ButtonInput.LightAttack)
    {
        if (activeAttack != null || entity._equipmentManager.equippedWeapon == null)
        {
            return;
        }

        Attack newAttack = entity._equipmentManager.equippedWeapon.GetNextAttack();
        if (newAttack == null)
        {
            return;
        }

        activeAttack = newAttack;

        StartCoroutine(newAttack.Activate(entity, button));

        

    }

    public void ActivateHeavyAttack(ButtonInput button = ButtonInput.HeavyAttack)
    {
        Debug.Log("ActivateHeavyAttack");
        if (activeAttack != null || entity._equipmentManager.equippedWeapon == null)
        {
            return;
        }

        Attack newAttack = entity._equipmentManager.equippedWeapon.GetHeavyAttack();
        if(newAttack == null)
        {
            return;
        }
        activeAttack = newAttack;

        StartCoroutine(newAttack.Activate(entity, button));
    }

    //This method fires a projectile at a certain angle (default to straight ahead
    public void FireProjectile(int angle = 90)
    {
        entity._equipmentManager.equippedWeapon.FireProjectile(angle);
    }

    //This method gets the players aim input
    public void FireAimedProjectile()
    {

        entity._equipmentManager.equippedWeapon.FireAimedProjectile();

    }

    public void FireRangedWeapon()
    {

    }

    public void ThrowWeapon()
    {
        entity._equipmentManager.equippedWeapon.ThrowWeapon();


    }

}
