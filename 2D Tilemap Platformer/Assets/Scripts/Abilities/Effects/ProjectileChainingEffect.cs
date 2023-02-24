using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileChainingEffect", menuName = "ScriptableObjects/Effects/ProjectileChainingEffect")]
public class ProjectileChainingEffect : Effect
{
    Projectile projectile;
    ProjectileData projData;
    Entity projOwner;

    public int maxChains = 3;
    public float chainRadius = 5;
    public ContactFilter2D contactFilter;

    Vector3 chainOrigin;
    Entity previousTarget;
    Entity chainTarget;

    public float chainDelay = 0.5f;
    public Chain chainPrefab;

    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(owner, effected, data);

        if(effectOwner is Projectile proj)
        {
            projectile = proj;
            projData = proj.projectileData;
            projData.attackData = proj._attackObject.GetAttackData();
            projOwner = proj._attackObject.owner;

            if (projectile != null)
            {

                Entity entity = FindChainTarget(effectedEntity.transform.position);

                if(entity != null)
                {
                    chainOrigin = proj.transform.position;
                    chainTarget = entity;
                    chainTarget.StartCoroutine(HandleChainEffect());
                }

            }
        }

    }

    public Entity FindChainTarget(Vector3 origin)
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(origin, chainRadius, contactFilter.layerMask);

        foreach (Collider2D collider in colliders)
        {
            Entity entity = collider.GetComponent<Entity>();

            if (entity is IHurtable hurtable)
            {

                if (entity == chainTarget || entity == effectedEntity || entity == projectile._attackObject.owner)
                {
                    continue;
                }

                return entity;
            }
        }

        return null;
    }

    public override void RemoveEffect()
    {

        base.RemoveEffect();
    }

    public IEnumerator HandleChainEffect()
    {
        float timeStamp = Time.time;
        Chain currentChain = Instantiate(chainPrefab);

        currentChain.startPos = chainOrigin;

        if (previousTarget)
        {
            currentChain.SetObjects(previousTarget.gameObject, chainTarget.gameObject);

        }
        else
        {
            currentChain.SetObjects(null, chainTarget.gameObject);

        }



        while (Time.time < timeStamp + chainDelay)
        {

            yield return null;
        }

        if(chainTarget is IHurtable hurtable)
        {
            AttackHitData hitData = projData.attackData.GetHitData(hurtable);

            hurtable.GetHurt(ref hitData);

            maxChains--;

            if(maxChains > 0)
            {
                Entity entity = FindChainTarget(chainTarget.transform.position);

                if(entity)
                {
                    previousTarget = chainTarget;
                    chainTarget = entity;
                    chainTarget.StartCoroutine(HandleChainEffect());
                }
            }
        }



        Destroy(currentChain.gameObject);


    }
}
