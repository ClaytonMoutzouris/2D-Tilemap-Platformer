using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExtraDamageOnHit", menuName = "ScriptableObjects/Abilities/ExtraDamageOnHit")]
public class ExtraDamageOnHit : Ability
{
    public int damage;


    public override void OnHit(AttackHitData hitData)
    {
        //This currently does pure damage, which doesn't proc onHurt/Onhit effects
        if (hitData.hit is Entity entity)
        {
            entity.health.LoseHealth(damage);
        }
    }
}