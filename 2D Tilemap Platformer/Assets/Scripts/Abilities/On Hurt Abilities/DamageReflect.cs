using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageReflect", menuName = "ScriptableObjects/Abilities/DamageReflect")]
public class DamageReflect : Ability
{
    public int reflectPercent = 100;

    public override void OnHurt(AttackHitData hitData)
    {
        //for now, call lose health with pure damage
        hitData.attackOwner.health.LoseHealth(hitData.damageDealt * (reflectPercent / 100));
    }
}
