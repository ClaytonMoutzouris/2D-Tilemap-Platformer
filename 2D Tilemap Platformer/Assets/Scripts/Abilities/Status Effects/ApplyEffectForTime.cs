using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplyEffectForTime", menuName = "ScriptableObjects/Effects/TimedEffects/ApplyEffectForTime")]
public class ApplyEffectForTime : StatusEffect
{
    public Effect effect;

    Effect activeEffect;

    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(owner, effected, data);

        if(effectedEntity)
        {
            activeEffect = Instantiate(effect);
            activeEffect.ApplyEffect(owner, effectedEntity, data);
        }
    }

    public override void RemoveEffect()
    {
        if(effectedEntity && activeEffect)
        {
            activeEffect.RemoveEffect();
        }

        base.RemoveEffect();
    }
}
