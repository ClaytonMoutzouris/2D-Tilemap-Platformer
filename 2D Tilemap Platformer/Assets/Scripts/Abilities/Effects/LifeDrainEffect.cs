using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LifeDrainEffect", menuName = "ScriptableObjects/Effects/LifeDrainEffect")]
public class LifeDrainEffect : Effect
{
    public int percentDrained = 100;

    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(owner, effected, data);

        if(effectedEntity is CharacterEntity character && data != null)
        {
            character.GetHealth().GainLife(data.damageDealt);
        }
    }


}
