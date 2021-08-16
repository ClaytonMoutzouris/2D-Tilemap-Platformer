using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TakeDamage", menuName = "ScriptableObjects/Effects/TakeDamage")]
public class TakeDamageEffect : ImmediateEffect
{
    public int baseDamage = 1;

    public override void Apply(Entity effected, Entity effector = null)
    {
        base.Apply(effected, effector);
        effected.health.LoseHealth(baseDamage);
    }

}
