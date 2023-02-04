using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageOverTime", menuName = "ScriptableObjects/StatusEffects/DamageOverTime")]
public class DamageOverTime : StatusEffect
{

    public float tickInterval = 1;
    public int damagePerTick = 1;



    public override IEnumerator HandleStatusEffect()
    {
        if(effectedEntity is IHurtable hurtable)
        {
            StartUp();
            float tickDuration = 0;

            while (Time.time <= timeStamp + duration + tickInterval)
            {

                tickDuration += Time.deltaTime;

                if (tickDuration >= tickInterval)
                {
                    hurtable.GetHealth().LoseHealth(damagePerTick);
                    tickDuration = 0;
                }
                yield return null;
            }

            EffectEnd();
        } 

    }
}
