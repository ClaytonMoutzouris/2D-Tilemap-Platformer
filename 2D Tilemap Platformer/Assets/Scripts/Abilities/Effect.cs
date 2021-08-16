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

    public virtual void Apply(Entity effected, Entity effector = null)
    {
        this.effected = effected;
        this.effector = effector;

    }

    public virtual void Remove()
    {
        //Remove from players list of effects, then garbage collect?
        //Destroy(this);
        effected = null;
        effector = null;
    }


}
