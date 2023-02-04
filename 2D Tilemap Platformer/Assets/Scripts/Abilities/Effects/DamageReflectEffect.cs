using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageReflectEffect", menuName = "ScriptableObjects/Effects/DamageReflectEffect")]
public class DamageReflectEffect : Effect
{
    public float percentReturned = 100;

    public override void ApplyEffect(Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(effected, data);

        if(data != null && data.attackOwner is CharacterEntity character)
        {
            character.GetHealth().LoseHealth(data.damageDealt);
        }

    }

}
