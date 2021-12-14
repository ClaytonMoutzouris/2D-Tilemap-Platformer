using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectTriggerType { Passive, OnAttack, OnHit, OnHurt, OnJump, OnRoll, Buff, Debuff, Aura };

public enum EffectType { Immediate, Passive, Buff, Activated }

[CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Effects/Effect")]
public class Effect : ScriptableObject
{

    protected Entity effected;
    protected Entity effector;
    public float procChance = 100;

    public void Trigger(Entity effected, Entity effector = null)
    {
        int bonusProcChance = (effector == null ? 0 : effector.stats.GetStat(StatType.Luck).GetValue());
        if (Random.Range(0, 100) >= procChance + bonusProcChance)
        {
            return;
        }

        this.effected = effected;
        this.effector = effector;

        ApplyEffect();
    }

    public virtual void ApplyEffect()
    {

    }


}
