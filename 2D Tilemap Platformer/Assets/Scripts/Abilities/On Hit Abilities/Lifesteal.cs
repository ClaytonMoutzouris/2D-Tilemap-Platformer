using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lifesteal", menuName = "ScriptableObjects/Abilities/Lifesteal")]
public class Lifesteal : Ability
{
    public int lifestealPercent = 100;

    public override void OnHit(AttackHitData hitData)
    {
        //This is a bit of a hack, might want to check for health component or maybe do another interface
        if (hitData.attackOwner is IHurtable hurtable)
        {
            int heals = hitData.damageDealt * (lifestealPercent / 100);
            hurtable.GetHealth().GainLife(heals);
        }

    }
}
