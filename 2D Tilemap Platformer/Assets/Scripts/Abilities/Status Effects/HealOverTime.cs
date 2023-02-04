﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealOverTime", menuName = "ScriptableObjects/StatusEffects/HealOverTime")]
public class HealOverTime : StatusEffect
{

    public float tickInterval = 1;
    public int healPerTick = 1;


    public override IEnumerator HandleStatusEffect()
    {
        if (effectedEntity is IHurtable hurtable)
        {
            StartUp();
            float tickDuration = 0;

            while (Time.time <= timeStamp + duration + tickInterval)
            {

                tickDuration += Time.deltaTime;

                if (tickDuration >= tickInterval)
                {
                    hurtable.GetHealth().GainLife(healPerTick);
                    tickDuration = 0;
                }
                yield return null;
            }

            EffectEnd();
        }
    }
}
