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
        Debug.Log("On Hurt");
        if (attackerEffect != null)
        {
            Effect effect = Instantiate(this.attackerEffect);

            effect.Trigger(attacker);
        }

        if (ownerEffect != null)
        {
            Effect effect = Instantiate(this.ownerEffect);

            effect.Trigger(owner);
        }

    }
}
