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
    public bool ignoreGravity = false;
    public bool knockedBack = false;

    public Health health;
    public Hurtbox hurtbox;
    //Class for organizing entities, which we may or may not need.
    public List<ParticleSystem> particleEffects = new List<ParticleSystem>();
    public Vector3 _velocity;
    public List<Ability> abilities = new List<Ability>();
    public List<Effect> statusEffects = new List<Effect>();

    public Stats stats;
    public bool isDead = false;


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
        floatingText.text.characterSize = floatingText.text.characterSize * sizeMult;
        floatingText.duration = dTime;
        floatingText.scrollSpeed = sSpeed;
        floatingText.GetComponent<TextMesh>().text = "" + text;
        floatingText.GetComponent<TextMesh>().color = color;
    }


    public virtual void OnKill(Entity killed)
    {

    }


    public virtual void GetHurt(AttackObject attackObject)
    {
        AttackData attackData = attackObject.GetAttackData();
        attackObject.attackData = attackData;

        float dodgeChance = stats.GetSecondaryStat(SecondaryStatType.DodgeChance).GetValue();

        int dodge = Random.Range(0, 100);

        if (dodge < dodgeChance)
        {
            ShowFloatingText("Dodged", Color.blue);
            return;
        }

        int fullDamage = attackData.GetDamage(this);

        health.LoseHealth(fullDamage, attackData.owner, attackData.crit);

        StartCoroutine(StatusEffects.Knockback(this, attackObject));
        

        foreach (Ability ability in attackObject.owner.abilities)
        {
            if (ability is EffectOnHit onHit)
            {
                onHit.OnHit(this);
            }
        }
    }
}
