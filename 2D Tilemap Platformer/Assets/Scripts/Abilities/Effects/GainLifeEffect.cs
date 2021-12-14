using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Effects/GainLife")]
public class GainLifeEffect : Effect
{
    public int baseLifeGain = 5;

    public override void ApplyEffect()
    {
        effected.health.GainLife(baseLifeGain);

    }

}
