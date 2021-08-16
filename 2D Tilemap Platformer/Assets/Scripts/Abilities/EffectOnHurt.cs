using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OnHurtAbility", menuName = "ScriptableObjects/Abilities/OnHurtAbility")]
public class EffectOnHurt : Ability
{
    public Effect ownerEffect;
    public Effect attackerEffect;

    public void OnHurt(Entity attacker)
    {
        if (attackerEffect != null)
        {
            Effect effect = Instantiate(this.attackerEffect);

            effect.Apply(attacker);
        }

        if (ownerEffect != null)
        {
            Effect effect = Instantiate(this.ownerEffect);

            effect.Apply(owner);
        }

    }
}
