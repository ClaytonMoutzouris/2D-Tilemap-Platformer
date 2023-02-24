using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectOnInterval", menuName = "ScriptableObjects/Effects/EffectOnInterval")]
public class EffectOnInterval : StatusEffect
{
    //The effect that we want to trigger on the interval
    public Effect triggeredEffect;
    public float interval;

    List<Effect> activeEffects = new List<Effect>();

    public override void RemoveEffect()
    {
        foreach(Effect effect in activeEffects)
        {
            if(effect != null)
            {
                effect.RemoveEffect();
            }
        }


        base.RemoveEffect();
    }

    public override IEnumerator HandleEffect()
    {
        timeStamp = Time.time;

        while (unlimitedDuration || Time.time < timeStamp + duration)
        {
            if(Time.time > timeStamp + interval)
            {

                Effect temp = Instantiate(triggeredEffect);
                temp.ApplyEffect(effectOwner, effectedEntity, attackHitData);
                activeEffects.Add(temp);
                timeStamp = Time.time;
            }
            yield return null;
        }

        RemoveEffect();
    }
}
