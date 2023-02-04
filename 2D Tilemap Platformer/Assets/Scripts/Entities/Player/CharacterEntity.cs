using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEntity : Entity, IHurtable
{

    public Sightbox sight;

    public Health health;
    public Hurtbox hurtbox;
    public Stats stats;

    public AttackManager _attackManager;


    protected override void Awake()
    {
        base.Awake();

        health = GetComponent<Health>();
        

        hurtbox = GetComponentInChildren<Hurtbox>();
        hurtbox.SetOwner(this);

        stats = GetComponent<Stats>();
        if (stats != null)
        {
            stats.Initialize();
        }
        health.UpdateHealth();

    }

    public Hurtbox GetHurtbox()
    {
        return hurtbox;
    }

    public Health GetHealth()
    {
        return health;
    }

    public virtual void GetHurt(ref AttackHitData hitData)
    {

        //This one reduced by 1% per damage reduction
        //int damageReduction = Mathf.FloorToInt((int)(finalDamage * ((hit.stats.GetSecondaryStat(SecondaryStatType.DamageReduction).GetValue()) / 100)));

        //This reduces damage by 1 for every 5 Damage Reduction (which is 1 to 1 with defense atm)
        int damageReduction = Mathf.FloorToInt(stats.GetSecondaryStat(SecondaryStatType.DamageReduction).GetValue() / 5);

        //we want to update the hitdata so it knows how much damage was actually dealt in the end
        hitData.damageDealt -= damageReduction;

        health.LoseHealth(hitData);

        foreach (Ability ability in abilities)
        {
            ability.OnHurt(hitData);
        }

        StartCoroutine(StatusEffects.Knockback(this, (transform.position - hitData.attackOwner.transform.position).normalized, hitData.knockbackPower));

    }

    public virtual bool CheckFriendly(Entity entity)
    {
        return entity == this;
    }

    public virtual bool CheckHit(AttackObject attackObject)
    {
        float dodgeChance = stats.GetSecondaryStat(SecondaryStatType.DodgeChance).GetValue();

        int dodge = Random.Range(0, 100);

        if (dodge < dodgeChance)
        {
            return false;
        }

        return true;
    }
}
