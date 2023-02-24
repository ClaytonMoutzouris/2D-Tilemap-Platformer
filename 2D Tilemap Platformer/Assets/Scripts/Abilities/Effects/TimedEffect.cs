using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectDurationType { Set, Unlimited }
[CreateAssetMenu(fileName = "TimedEffect", menuName = "ScriptableObjects/Effects/TimedEffect")]
public class TimedEffect : Effect
{
    [Header("Timed Effect Paramaters")]
    public float duration = 5;
    public bool unlimitedDuration = false;
    public bool stackable = false;
    public int stackLimit = 5;
    protected float timeStamp;

    public ParticleSystem effectPrefab;
    ParticleSystem activeSystem;


    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        if(!CheckRequirements(owner, effected)){
            return;
        }

        base.ApplyEffect(owner, effected, data);


        effectedEntity.continuousEffects.Add(this);
        if(effectPrefab)
        {
            activeSystem = effectedEntity.AddEffect(effectPrefab);
        }

        effectedEntity.StartCoroutine(HandleEffect());

    }

    public override void RemoveEffect()
    {
        base.RemoveEffect();


        if (effectedEntity)
        {
            effectedEntity.continuousEffects.Remove(this);

            if (activeSystem)
            {
                effectedEntity.RemoveEffect(activeSystem);
            }
        }


    }


    public virtual IEnumerator HandleEffect()
    {

        timeStamp = Time.time;

        while (unlimitedDuration || Time.time < timeStamp + duration)
        {
            if (!effectedEntity)
            {
                break;
            }

            //Do stuff

            yield return null;
        }

        RemoveEffect();

    }

    public override bool CheckRequirements(Entity owner, Entity effected)
    {
        bool canApply = base.CheckRequirements(owner, effected);

        if (!stackable)
        {
            foreach (TimedEffect effect in effected.continuousEffects)
            {
                if (effect.name.Equals(name))
                {
                    //This will refresh the cooldown instead. might not want to do that for some things.
                    effect.timeStamp = Time.time;
                    canApply = false;
                }
            }
        }

        return canApply;

    }
}
