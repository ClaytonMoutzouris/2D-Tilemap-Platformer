using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PullEffect", menuName = "ScriptableObjects/Effects/PullEffect")]
public class ChargingEffect : TimedEffect
{

    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(owner, effected, data);

        if (effectedEntity)
        {

        }

    }

    public override IEnumerator HandleEffect()
    {
        timeStamp = Time.time;

        while (unlimitedDuration || Time.time < timeStamp + duration)
        {
            if (!effectedEntity)
            {
                break;
            }


            yield return null;
        }

        RemoveEffect();
    }

    public override void RemoveEffect()
    {
        base.RemoveEffect();
    }

    public override bool CheckRequirements(Entity owner, Entity effected)
    {
        bool canApply = base.CheckRequirements(owner, effected);

        return canApply;
    }
}
