using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageReflect", menuName = "ScriptableObjects/Abilities/DamageReflect")]
public class DamageReflect : Ability
{
    public int reflectPercent = 100;

    public override void OnHurt(AttackData attackData)
    {
        //for now, call lose health with pure damage
        attackData.owner.health.LoseHealth(attackData.damage * (reflectPercent / 100));
    }
}
