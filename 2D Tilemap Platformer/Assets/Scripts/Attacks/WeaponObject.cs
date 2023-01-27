using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : AttackObject
{
    public Weapon weapon;

    public void SetWeapon(Weapon newWeapon)
    {
        weapon = newWeapon;
        weapon.SetObject(this);
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
        AttackData data = weapon.GetAttackData();
        data.owner = owner;
        return data;
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
        
        //If i want to change the size
        //transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * weapon.GetStatValue(WeaponAttributesType.WeaponReach), transform.localScale.z);
    }


    public void AimWeapon(Vector2 aim)
    {

        transform.localScale = new Vector3((int)owner.GetDirection() * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        if (owner.GetDirection() > 0)
        {
            spriteRenderer.flipY = false;
        }
        else
        {
            spriteRenderer.flipY = true;
        }

        float angle = Mathf.Atan2(aim.x, aim.y) * Mathf.Rad2Deg - 90;

        transform.eulerAngles = new Vector3(0, 0, -angle);
    }

    public override void HitEnemy(IHurtable hit)
    {
        //make sure to refresh this
        if (!hit.CheckHit(this))
        {
            return;
        }

        attackData = GetAttackData();

        base.HitEnemy(hit);

        weapon.ComboUp();


    }
}
