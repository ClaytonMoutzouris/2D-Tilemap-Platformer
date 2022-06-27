using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : ScriptableObject
{
    [HideInInspector]
    public Entity effectedEntity;
    //Might not need this one, but will keep it around for now
    [HideInInspector]
    public Entity effectOwner;
    public int duration = 5;
    public bool stackable = false;
    [HideInInspector]
    public float timeStamp;

    public void ApplyEffect(Entity effected, Entity owner = null)
    {
        //Inverse this later
        if (!stackable)
        {
            foreach (StatusEffect status in effected.statusEffects)
            {
                if (status.name.Equals(name))
                {
                    status.timeStamp = Time.time;
                    return;
                }
            }
        }

        effectedEntity = effected;
        effectOwner = owner;

        //Run the coroutine on the entity
        effectedEntity.StartCoroutine(HandleStatusEffect());
    }

    public virtual void StartUp()
    {
        effectedEntity.statusEffects.Add(this);
        timeStamp = Time.time;

    }

    public virtual void RemoveEffect()
    {
        EffectEnd();
    }

    public virtual void EffectEnd()
    {
        //Might need a better way to exit the coroutine
        //effected.StopCoroutine(HandleStatusEffect());

        effectedEntity.statusEffects.Remove(this);
        effectedEntity = null;
    }

    public virtual IEnumerator HandleStatusEffect()
    {
        StartUp();

        while (Time.time < timeStamp + duration)
        {

            yield return null;
        }

        EffectEnd();

    }
}
