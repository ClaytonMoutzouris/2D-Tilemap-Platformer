using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectOnWalk", menuName = "ScriptableObjects/Abilities/OnWalkAbility")]
public class EffectOnWalk : Ability
{
    public Effect walkEffect;
    float walkCount = 0;
    Vector2 previousPosition = Vector2.zero;
    public float walkThreshold = 1;

    public void OnWalk()
    {
        walkCount += Mathf.Abs(owner.transform.position.x - previousPosition.x);

        previousPosition = owner.transform.position;

        if (walkCount >= walkThreshold)
        {
            if (walkEffect != null)
            {
                Effect walkEffect = Instantiate(this.walkEffect);

                walkEffect.Apply(owner);
                walkCount = 0;

            }
        }



    }
}
