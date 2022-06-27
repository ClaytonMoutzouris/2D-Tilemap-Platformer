using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExtraDamageOnHit", menuName = "ScriptableObjects/Abilities/ExtraDamageOnHit")]
public class ExtraDamageOnHit : Ability
{
    public int damage;


    public override void OnHit(AttackData attackData, Entity hitEntity)
    {
        //This currently does pure damage, which doesn't proc onHurt/Onhit effects
        hitEntity.health.LoseHealth(damage);
    }
}