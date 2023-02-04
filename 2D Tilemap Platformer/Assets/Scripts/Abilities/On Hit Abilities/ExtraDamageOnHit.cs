using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExtraDamageOnHit", menuName = "ScriptableObjects/Abilities/ExtraDamageOnHit")]
public class ExtraDamageOnHit : Ability
{
    public int damage;


    public override void OnHit(AttackHitData hitData)
    {

        hitData.hit.GetHealth().LoseHealth(damage);
        
    }
}