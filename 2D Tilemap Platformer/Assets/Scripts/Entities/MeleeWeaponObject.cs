using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponObject : MonoBehaviour
{
    public AttackObject attackObject;


    public void Start()
    {
        attackObject = GetComponent<AttackObject>();
    }

    public void SetWeapon(Weapon weapon)
    {
        attackObject.spriteRenderer.sprite = weapon.sprite;

    }


}
