using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : AttackObject
{

    public void SetWeapon(Weapon weapon)
    {
        //gameObject.SetActive(true);
        Debug.Log("Sprite " + weapon.sprite.ToString());
        spriteRenderer.sprite = weapon.sprite;
        Debug.Log("Sprite Renderer " + spriteRenderer.sprite.ToString());

        hitbox.size = weapon.weaponBase.weaponObjectPrototype.hitbox.size;
        hitbox.offset = weapon.weaponBase.weaponObjectPrototype.hitbox.offset;
        //gameObject.SetActive(false);
        damage = weapon.damage;
        knockbackPower = weapon.knockbackPower;
        //Add and set the animator
        if (animator == null)
        {
            animator = gameObject.AddComponent<Animator>();
        }

        animator.runtimeAnimatorController = weapon.weaponBase.weaponObjectPrototype.animator.runtimeAnimatorController;

    }

}
