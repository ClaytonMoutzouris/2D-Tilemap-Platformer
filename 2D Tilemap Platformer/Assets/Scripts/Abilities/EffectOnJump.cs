using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectOnJump", menuName = "ScriptableObjects/Abilities/EffectOnJump")]
public class EffectOnJump : Ability
{
    public Effect jumpEffect;

    public void OnJump()
    {

        if (jumpEffect != null)
        {
            Effect jumpEffect = Instantiate(this.jumpEffect);

            jumpEffect.Trigger(owner);

        }

    }
}
