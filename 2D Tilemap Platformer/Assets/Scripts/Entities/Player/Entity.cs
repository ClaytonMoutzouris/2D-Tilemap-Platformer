using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityDirection { Left = -1, Right = 1 };
public enum AbilityFlagType { Flight, Stealth }

[RequireComponent(typeof(Animator))]
public class Entity : MonoBehaviour, IHurtable
{
    public SpriteRenderer spriteRenderer;
    public Animator _animator;
    public AnimatorOverrideController overrideController;
    public AttackManager _attackManager;

    //Only used by enemy now, need to remove
    public float movementSpeed = 0.0f;

    //Entity Flags

    public Health health;
    public Hurtbox hurtbox;
    //Class for organizing entities, which we may or may not need.
    public List<ParticleSystem> particleEffects = new List<ParticleSystem>();

    #region MoveToController
    //public Vector3 _velocity;
    public bool knockedBack = false;
    #endregion

    public List<Ability> abilities = new List<Ability>();
    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    public Stats stats;
    public bool isDead = false;

    public PhysicsBody2D _controller;


    // Start is called before the first frame update
    void Start()
    {

        

    }

    protected virtual void Awake()
    {
        health = GetComponent<Health>();
        hurtbox = GetComponentInChildren<Hurtbox>();
        hurtbox.SetOwner(this);
        _animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        stats = GetComponent<Stats>();
        if (stats != null)
        {
            stats.Initialize();
            health.UpdateHealth();

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ParticleSystem AddEffect(ParticleSystem effectPrefab)
    {
        ParticleSystem newEffect = Instantiate(effectPrefab, transform);
        particleEffects.Add(newEffect);
        return newEffect;
    }

    public void RemoveEffect(ParticleSystem effect)
    {
        particleEffects.Remove(effect);
        if(effect != null)
        {
            Destroy(effect.gameObject);
        }
    }

    public int GetDirection()
    {
        return 1 * (int)Mathf.Sign(transform.localScale.x);
    }

    public virtual void SetDirection(EntityDirection dir)
    {
        if (transform.localScale.x < 0f && dir == EntityDirection.Right || transform.localScale.x > 0f && dir == EntityDirection.Left)
        {
            transform.localScale = new Vector3((int)dir * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public virtual void Die()
    {
        isDead = true;
    }

    public virtual void ShowFloatingText(string text, Color color, float dTime = 1, float sSpeed = 1, float sizeMult = 1.0f)
    {
        FloatingText floatingText = GameObject.Instantiate(Resources.Load<FloatingText>("Prefabs/UI/FloatingText") as FloatingText, transform.position, Quaternion.identity);
        //floatingText.SetOffset(Vector3.up * (Body.mAABB.HalfSize.y * 2 + 10));
        floatingText.text.fontSize = floatingText.text.fontSize * sizeMult;
        floatingText.duration = dTime;
        floatingText.scrollSpeed = sSpeed;
        floatingText.SetText(text);
        floatingText.SetColor(color);
    }


    public virtual void OnKill(Entity killed)
    {

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

    public Health GetHealth()
    {
        throw new System.NotImplementedException();
    }

    public Hurtbox GetHurtbox()
    {
        throw new System.NotImplementedException();
    }

    public Entity GetEntity()
    {
        return this;
    }
}
