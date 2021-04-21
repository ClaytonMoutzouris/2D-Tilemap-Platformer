using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAttackID { Basic }

public class PlayerAttackManager : MonoBehaviour
{
    //List of different player attack animations
    public PlayerController player;
    public List<AttackData> attacks = new List<AttackData>();
    public Attack activeAttack;

    //Where do you really need to be?
    public WeaponRenderer meleeWeapon;
    //public WeaponRenderer rangedWeapon;

    public int attackIndex = 0;
    //Right now its not, but this should follow up off the end of an attack, like a cooldown.
    public float followUpThreshold = 1f;
    public float lastAttackTime = 0;

    public List<Weapon> weapons;
    public int weaponIndex = 0;
    public Weapon equippedWeapon;


    private void Start()
    {
        player = GetComponent<PlayerController>();
        SetWeapon(weapons[weaponIndex]);

    }

    public void SetWeapon(Weapon weapon)
    {
        equippedWeapon = weapon;
        AttackObject attackObject = equippedWeapon.weaponObject.GetComponent<AttackObject>();
        if(attackObject)
            equippedWeapon.attack.SetObject(attackObject);

        //meleeWeapon.SetWeapon(equippedWeapon.weaponObject);

        if (equippedWeapon is RangedWeapon rangedWeapon)
        {
            ((RangedWeaponAttack)rangedWeapon.attack).SetProjectile(rangedWeapon.projectile);
        }
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

        equippedWeapon = weapons[index];
        //meleeWeapon.SetWeapon(equippedWeapon.weaponObject);

        AttackObject attackObject = equippedWeapon.weaponObject.GetComponent<AttackObject>();
        if (attackObject)
            equippedWeapon.attack.SetObject(attackObject);
    }

    public bool IsAttacking()
    {

            if(activeAttack != null)
            {
                return true;
            }
        

        return false;
    }

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

    public void ActivateWeaponAttack()
    {
        if (activeAttack != null)
        {
            return;
        }

        Attack newAttack = Instantiate(equippedWeapon.attack, transform);
        StartCoroutine(newAttack.Activate(player));
        activeAttack = newAttack;
        lastAttackTime = Time.time;

    }

    public void ActivateAttack()
    {
        if(activeAttack != null)
        {
            return;
        }
        
        if(Time.time >= lastAttackTime + followUpThreshold)
        {
            attackIndex = 0;
        }


        Attack newAttack = Instantiate(attacks[attackIndex].attack, transform);
        StartCoroutine(newAttack.Activate(player));
        activeAttack = newAttack;
        lastAttackTime = Time.time;

        attackIndex++;
        if(attackIndex >= attacks.Count)
        {
            attackIndex = 0;
        }
    }


}
