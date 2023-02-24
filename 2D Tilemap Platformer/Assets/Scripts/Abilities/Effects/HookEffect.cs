using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HookEffect", menuName = "ScriptableObjects/Effects/HookEffect")]
public class HookEffect : StatusEffect
{
    public float hookSpeed = 2;
    public Vector3 hookOffset;

    public Chain chainPrefab;
    Chain activeChain;

    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(owner, effected, data);

        if (effectedEntity)
        {
            activeChain = Instantiate(chainPrefab);
            activeChain.SetObjects(effectedEntity.gameObject, effectOwner.gameObject);
        }
    }

    public override void RemoveEffect()
    {
        if(activeChain)
        {
            Destroy(activeChain.gameObject);
        }

        base.RemoveEffect();
    }

    public override IEnumerator HandleEffect()
    {

        timeStamp = Time.time;

        while (unlimitedDuration || Time.time < timeStamp + duration)
        {
            if(!effectedEntity)
            {
                break;
            }

            Vector3 direction = (effectOwner.transform.position - effectedEntity.transform.position).normalized;

            effectedEntity._controller.velocity = direction * hookSpeed;


            yield return null;
        }

        RemoveEffect();
    }


    public override bool CheckRequirements(Entity owner, Entity effected)
    {
        bool canApply = base.CheckRequirements(owner, effected);

        if(effected.isDead || effected._controller.isKinematic)
        {
            canApply = false;
        }

        return canApply;

    }
}
