using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TriggeredEffect", menuName = "ScriptableObjects/Effects/TriggeredEffect")]
public class TriggeredEffect : Effect
{

    //maybe add a cooldown or something here

    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(owner, effected, data);

        Trigger(owner, data);
    }

    public virtual void Trigger(Entity owner, AttackHitData data = null)
    {

    }

}
