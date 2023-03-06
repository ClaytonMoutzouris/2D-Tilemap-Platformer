using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreatePortalEffect", menuName = "ScriptableObjects/Effects/CreatePortalEffect")]
public class CreatePortalEffect : TriggeredEffect
{
    public Portal prefab;

    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(owner, effected, data);


        if(effectedEntity)
        {
            Portal portal = Instantiate(prefab, effected.transform.position, Quaternion.identity);
            portal.SetOwner(owner);
            
        }


    }

}
