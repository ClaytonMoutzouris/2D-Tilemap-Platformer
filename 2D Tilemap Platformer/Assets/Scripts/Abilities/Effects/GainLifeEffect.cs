using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Effects/GainLife")]
public class GainLifeEffect : ImmediateEffect
{
    public int baseLifeGain = 5;

    public override void Apply(Entity effected, Entity effector = null)
    {
        base.Apply(effected, effector);
        effected.health.GainLife(baseLifeGain);
    }

}
