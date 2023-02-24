using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplyEffectAfterTime", menuName = "ScriptableObjects/Effects/TimedEffects/ApplyEffectAfterTime")]
public class ApplyEffectAfterTime : StatusEffect
{
    public Effect effect;

    Effect activeEffect;

    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(owner, effected, data);

    }

    public override void RemoveEffect()
    {
        if (effectedEntity && activeEffect)
        {
            activeEffect.RemoveEffect();
        }

        base.RemoveEffect();
    }

    public override IEnumerator HandleEffect()
    {
        timeStamp = Time.time;

        while (Time.time < timeStamp + duration)
        {
            if (!effectedEntity)
            {
                break;
            }

            yield return null;
        }

        if (effectedEntity)
        {
            activeEffect = Instantiate(effect);
            activeEffect.ApplyEffect(effectOwner, effectedEntity, attackHitData);
        }

        RemoveEffect();
    }
}
