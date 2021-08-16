using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OnKillAbility", menuName = "ScriptableObjects/Abilities/OnKillAbility")]
public class EffectOnKill : Ability
{
    public Effect gainEffect;

    public void OnKill(Entity killed)
    {

        if (gainEffect != null)
        {
            Effect gainEffect = Instantiate(this.gainEffect);

            gainEffect.Apply(owner);
        }

    }
}
