using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : AttackObject
{
    public Weapon weapon;

    public void SetWeapon(Weapon newWeapon)
    {
        weapon = newWeapon;
        spriteRenderer.sprite = weapon.sprite;
        spriteRenderer.color = weapon.color;

        UpdateHitbox();

        animator = GetComponent<Animator>();
        //Add and set the animator
        if (animator == null)
        {
            animator = gameObject.AddComponent<Animator>();

        }

        animator.runtimeAnimatorController = weapon.weaponBase.animator.runtimeAnimatorController;

    }

    public override AttackData GetAttackData()
    {
        return weapon.GetAttackData();
    }

    public void UpdateHitbox()
    {
        if(weapon == null)
        {
            return;
        }

        Vector2 size = weapon.weaponBase.hitbox.size + Vector2.up * weapon.GetStatValue(WeaponAttributesType.WeaponReach);
        Vector2 offset = weapon.weaponBase.hitbox.offset + Vector2.up * weapon.GetStatValue(WeaponAttributesType.WeaponReach) / 2;
        hitbox.size = size;
        hitbox.offset = offset;
    }

}
