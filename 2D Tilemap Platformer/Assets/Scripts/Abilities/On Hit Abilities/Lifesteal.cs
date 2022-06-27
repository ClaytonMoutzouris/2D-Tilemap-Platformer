using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lifesteal", menuName = "ScriptableObjects/Abilities/Lifesteal")]
public class Lifesteal : Ability
{
    public int lifestealPercent = 100;

    public override void OnHit(AttackData attackData, Entity hitEntity)
    {
        int heals = attackData.damage * (lifestealPercent / 100);
        owner.health.GainLife(heals);
    }
}
