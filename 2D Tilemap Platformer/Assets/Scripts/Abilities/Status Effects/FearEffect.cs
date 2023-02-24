using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FearEffect", menuName = "ScriptableObjects/StatusEffects/FearEffect")]
public class FearEffect : StatusEffect
{

    public override IEnumerator HandleEffect()
    {

        Vector2 runVector = effectOwner.transform.position - effectedEntity.transform.position;

        while (unlimitedDuration || Time.time <= timeStamp + duration)
        {
            //effectedEntity.normalized;

            yield return null;
        }

        RemoveEffect();

    }
}
