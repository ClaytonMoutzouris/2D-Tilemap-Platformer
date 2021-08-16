using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectOverTime", menuName = "ScriptableObjects/Effects/StatusEffects/EffectOverTime")]
public class EffectOverTime : StatusEffect
{
    public int tickInterval;
    public Effect effect;

    public override void StartUp()
    {
        base.StartUp();

    }

    public override void EffectEnd()
    {

        base.EffectEnd();
    }

    public override IEnumerator HandleEffect()
    {
        StartUp();
        float tickDuration = 0;

        while (Time.time <= timeStamp + duration + tickInterval)
        {
            tickDuration += Time.deltaTime;

            if (tickDuration >= tickInterval)
            {
                Effect newEffect = Instantiate(effect);
                newEffect.Apply(effected, effector);
                tickDuration = 0;
            }
            yield return null;
        }

        EffectEnd();

    }
}
