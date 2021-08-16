using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RapidRangedAttack", menuName = "ScriptableObjects/Attacks/RapidRangedAttack")]
public class RapidRangedAttack : RangedAttack
{
    //move these to the parent class
    float lastFiredTimestamp = 0;

    //A basic attack.
    public override IEnumerator Activate(PlayerController user, ButtonInput button = ButtonInput.Fire)
    {
        entity = user;
        StartUp();
        entity._animator.speed = attackSpeed;

        while (user._input.GetButton(button))
        {
            entity._animator.Play(attackAnimation.name);
            Vector2 aim = user._input.GetRightStickAim();

            if (aim == Vector2.zero)
            {
                aim = user.GetDirection() * Vector2.right;
            }

            float angle = Mathf.Atan2(aim.x, aim.y) * Mathf.Rad2Deg - 90;



            WeaponObject weaponObj = entity._attackManager.rangedWeaponObject;

            weaponObj.transform.localScale = new Vector3((int)user.GetDirection() * Mathf.Abs(weaponObj.transform.localScale.x), weaponObj.transform.localScale.y, weaponObj.transform.localScale.z);

            if(user.GetDirection() > 0)
            {
                weaponObj.spriteRenderer.flipY = false;
            } else
            {
                weaponObj.spriteRenderer.flipY = true;
            }

            weaponObj.transform.eulerAngles = new Vector3(0, 0, -angle);


            if (Time.time > lastFiredTimestamp + (1/ user._equipmentManager.equippedWeapon.GetStatValue(WeaponAttributesType.FireRate)))
            {

                lastFiredTimestamp = Time.time;
                user._equipmentManager.equippedWeapon.FireAimedProjectile();
            }
            yield return null;
        }



        entity._animator.speed = 1;
        entity._animator.Play(Animator.StringToHash("Idle"));
        CleanUp();
    }
}
