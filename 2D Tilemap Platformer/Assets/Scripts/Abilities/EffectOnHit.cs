using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OnHitAbility", menuName = "ScriptableObjects/Abilities/OnHitAbility")]
public class EffectOnHit : Ability
{
    public Effect hitEffect;
    public Effect gainEffect;

    public void OnHit(Entity hit)
    {
        if(hitEffect != null)
        {
            Effect effect = Instantiate(hitEffect);

            effect.Apply(hit, owner);
        }

        if (gainEffect != null)
        {
            Effect gainEffect = Instantiate(this.gainEffect);

            gainEffect.Apply(owner);
        }

    }
}
