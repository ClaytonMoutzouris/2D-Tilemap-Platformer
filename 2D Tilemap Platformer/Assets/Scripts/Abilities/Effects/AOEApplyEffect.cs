using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AOEApplyEffect", menuName = "ScriptableObjects/Effects/AOEApplyEffect")]
public class AOEApplyEffect : Effect
{

    public Effect effectToApply;
    public ContactFilter2D contactFilter;
    public float radius = 4;
    public bool applyToOwner = false;
    public bool applyToEffected = false;

    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(owner, effected, data);


        Collider2D[] colliders = Physics2D.OverlapCircleAll(effected.transform.position, radius, contactFilter.layerMask);
        

        foreach(Collider2D collider in colliders)
        {
            Entity entity = collider.GetComponent<Entity>();

            if(entity)
            {

                if(entity == effectOwner && !applyToOwner || entity == effected && !applyToEffected)
                {
                    continue;
                }

                Effect temp = Instantiate(effectToApply);
                temp.ApplyEffect(effectOwner, entity, data);
            }
        }

    }
}
