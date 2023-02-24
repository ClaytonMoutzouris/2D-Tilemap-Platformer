using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Not a lot of purpose for this at the moment.
public class AuraEffect : TimedEffect
{
    public Effect effect;

    public List<Entity> buffedEntities;

    public override IEnumerator HandleEffect()
    {
        return base.HandleEffect();
    }

    public override void RemoveEffect()
    {
        base.RemoveEffect();
    }
}
