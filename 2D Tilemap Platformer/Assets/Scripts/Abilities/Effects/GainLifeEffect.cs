using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GainLifeEffect", menuName = "ScriptableObjects/Effects/GainLifeEffect")]
public class GainLifeEffect : Effect
{
    public int lifeGained = 5;

    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(owner, effected, data);

        if(effectedEntity is CharacterEntity character)
        {
            character.GetHealth().GainLife(lifeGained);
        }
    }

    public void SetLifeGained(int value)
    {
        lifeGained = value;
    }
}
