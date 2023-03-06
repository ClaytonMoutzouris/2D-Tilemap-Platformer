using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityDirection { Left = -1, Right = 1 };
public enum AbilityFlagType { Flight, Stealth }

[RequireComponent(typeof(Animator))]
public class Entity : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator _animator;
    public AnimatorOverrideController overrideController;

    //Only used by enemy now, need to remove

    //Entity Flags
    //Class for organizing entities, which we may or may not need.
    public List<ParticleSystem> particleEffects = new List<ParticleSystem>();

    #region MoveToController
    //public Vector3 _velocity;
    public bool knockedBack = false;
    #endregion

    public List<Ability> abilities = new List<Ability>();
    public List<TimedEffect> continuousEffects = new List<TimedEffect>();
    public List<WeaponBonusEffect> weaponEffects = new List<WeaponBonusEffect>();

    public bool isDead = false;

    public PhysicsBody2D _controller;

    // Start is called before the first frame update
    void Start()
    {


    }

    protected virtual void Awake()
    {

        _animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _controller = GetComponent<PhysicsBody2D>();
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
        StopAllCoroutines();


        List<Effect> tempList = new List<Effect>();
        tempList.AddRange(continuousEffects);

        foreach(TimedEffect effect in tempList)
        {
            effect.RemoveEffect();
        }
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

    /*
    public virtual void GetHurt(ref AttackHitData hitData)
    {

    }
    */

    /*
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
    */

    public Entity GetEntity()
    {
        return this;
    }

    public virtual float GetGravityModifier()
    {
        return GambleConstants.GRAVITY*_controller.gravityMod;
    }
}
