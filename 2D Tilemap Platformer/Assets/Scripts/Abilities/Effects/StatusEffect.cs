using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "ScriptableObjects/Effects/StatusEffects/StatusEffect")]
public class StatusEffect : Effect
{
    public int duration;
    public bool stackable = false;
    [HideInInspector]
    public float timeStamp;

    public List<StatBonus> statBonuses;
    public List<SecondaryStatBonus> secondaryStatBonus;

    public override void Apply(Entity effected, Entity effector = null)
    {
        if (!stackable)
        {
            foreach (Effect effect in effected.statusEffects)
            {
                if (effect is StatusEffect statusEffect && statusEffect.name.Equals(name))
                {
                    Debug.Log("Prevented stacking");
                    statusEffect.timeStamp = Time.time;
                    return;
                }
            }
        }


        base.Apply(effected, effector);
        effected.StartCoroutine(HandleEffect());
    }

    public virtual void StartUp()
    {
        effected.statusEffects.Add(this);
        timeStamp = Time.time;
        if(effected.stats != null)
        {
            effected.stats.AddPrimaryBonuses(statBonuses);
            effected.stats.AddSecondaryBonuses(secondaryStatBonus);
        }

    }

    public virtual void EffectEnd()
    {
        if (effected.stats != null)
        {
            effected.stats.RemovePrimaryBonuses(statBonuses);
            effected.stats.RemoveSecondaryBonuses(secondaryStatBonus);
        }

        //Remove();
        effected.statusEffects.Remove(this);
    }

    public virtual IEnumerator HandleEffect()
    {
        StartUp();

        while (Time.time < timeStamp + duration)
        {

            yield return null;
        }

        EffectEnd();

    }
}
