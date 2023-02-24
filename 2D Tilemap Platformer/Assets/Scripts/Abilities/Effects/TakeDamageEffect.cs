using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TakeDamageEffect", menuName = "ScriptableObjects/Effects/TakeDamageEffect")]
public class TakeDamageEffect : Effect
{
    public AttackData attackData;
    public bool triggerAbilities = false;

    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(owner, effected, data);

        if(effectedEntity is CharacterEntity character)
        {
            attackData.owner = effectOwner;
            AttackHitData hitData = attackData.GetHitData(character);

            character.GetHurt(ref hitData);

            if(triggerAbilities)
            {
                foreach (Ability ability in effectOwner.abilities)
                {
                    ability.OnHit(hitData);
                }
            }
        }
    }

    public void SetAttackData(AttackData value)
    {
        attackData = value;
    }
}
