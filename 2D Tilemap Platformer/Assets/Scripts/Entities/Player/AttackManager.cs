using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    //List of different player attack animations
    public PlayerController player;
    //public List<AttackData> attacks = new List<AttackData>();
    public Attack activeAttack;

    //Where do you really need to be?
    public WeaponObject meleeWeaponObject;
    public WeaponObject rangedWeaponObject;

    public GameObject RotationObject;

    //Right now its not, but this should follow up off the end of an attack, like a cooldown.
    public float followUpThreshold = 1f;
    public float lastAttackTime = 0;



    private void Start()
    {
        player = GetComponent<PlayerController>();
        List<Weapon> tempList = new List<Weapon>();
        //player._animator.keepAnimatorControllerStateOnDisable = true;
    }

    public void Update()
    {

        if (player.movementState != PlayerMovementState.GrabLedge && player.movementState != PlayerMovementState.Roll
            && player.movementState != PlayerMovementState.Dead && !player.knockedBack)
        {

            if (player._input.GetButtonDown(ButtonInput.Attack_Down))
            {
                ActivateAttack(AttackInput.Down, ButtonInput.LightAttack);
            }

            if (player._input.GetButtonDown(ButtonInput.Attack_Up))
            {
                ActivateAttack(AttackInput.Up, ButtonInput.LightAttack);
            }

            if ((player._input.GetButtonDown(ButtonInput.Attack_Left) && player.GetDirection() == -1) || 
                (player._input.GetButtonDown(ButtonInput.Attack_Right) && player.GetDirection() == 1))
            {
                ActivateAttack(AttackInput.Forward, ButtonInput.LightAttack);
            }

            if ((player._input.GetButtonDown(ButtonInput.Attack_Left) && player.GetDirection() == 1) ||
                (player._input.GetButtonDown(ButtonInput.Attack_Right) && player.GetDirection() == -1))
            {
                ActivateAttack(AttackInput.Backward, ButtonInput.LightAttack);
            }

            if (player._input.GetButtonDown(ButtonInput.Attack_Neutral))
            {
                ActivateAttack(AttackInput.Neutral, ButtonInput.LightAttack);
            }

            if (player._input.GetButtonDown(ButtonInput.HeavyAttack))
            {
                ActivateHeavyAttack();
            }

            if (player._input.GetButtonDown(ButtonInput.Fire))
            {
                ActivateAttack(AttackInput.Shoot, ButtonInput.Fire);
            }

        }

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

    public void ActivateAttack(AttackInput attackInput, ButtonInput buttonInput)
    {
        if (activeAttack != null)
        {
            return;
        }

        if(buttonInput == ButtonInput.Fire)
        {
            Weapon wep = player._equipmentManager.GetEquippedWeapon(WeaponSlot.Ranged);

            if(!wep)
            {
                return;
            }

            WeaponAttack newAttack = wep.GetAttack(attackInput);
            if (newAttack == null)
            {
                return;
            }

            newAttack.SetAttacker(player);
            newAttack.SetWeapon(wep);
            activeAttack = newAttack;

            StartCoroutine(newAttack.Activate(buttonInput));
        }
        else
        {
            Weapon wep = player._equipmentManager.GetEquippedWeapon(WeaponSlot.Melee);
            if (!wep)
            {
                return;
            }

            WeaponAttack newAttack = wep.GetAttack(attackInput);
            if (newAttack == null)
            {
                return;
            }
            newAttack.SetAttacker(player);
            newAttack.SetWeapon(wep);

            activeAttack = newAttack;

            StartCoroutine(newAttack.Activate(buttonInput));
        }


        

    }

    public void ActivateHeavyAttack(ButtonInput button = ButtonInput.HeavyAttack)
    {
        if (activeAttack != null || player._equipmentManager.GetEquippedWeapon(WeaponSlot.Melee) == null)
        {
            return;
        }

        WeaponAttack newAttack = player._equipmentManager.GetEquippedWeapon(WeaponSlot.Melee).GetHeavyAttack();
        if(newAttack == null)
        {
            return;
        }
        newAttack.SetAttacker(player);
        newAttack.SetWeapon(player._equipmentManager.GetEquippedWeapon(WeaponSlot.Melee));

        activeAttack = newAttack;

        StartCoroutine(newAttack.Activate(button));
    }

    //This method fires a projectile at a certain angle (default to straight ahead
    public void FireProjectile(int angle = 90)
    {
        player._equipmentManager.GetEquippedWeapon(WeaponSlot.Ranged).FireProjectile(angle);
    }

    //This method gets the players aim input
    public void FireAimedProjectile()
    {

        player._equipmentManager.GetEquippedWeapon(WeaponSlot.Ranged).FireAimedProjectile();

    }

    public void FireRangedWeapon()
    {

    }

    public void ThrowWeapon(WeaponSlot slot)
    {

        player._equipmentManager.GetEquippedWeapon(slot).ThrowWeapon();


    }

}
