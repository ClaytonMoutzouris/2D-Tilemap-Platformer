using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageOverTime", menuName = "ScriptableObjects/StatusEffects/DamageOverTime")]
public class FearEffect : StatusEffect
{

    public override IEnumerator HandleStatusEffect()
    {
        StartUp();

        Vector2 runVector = effectOwner.transform.position - effectedEntity.transform.position;

        while (Time.time <= timeStamp + duration)
        {
            //effectedEntity.normalized;

            yield return null;
        }

        EffectEnd();

    }
}
