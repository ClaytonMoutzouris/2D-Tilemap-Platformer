using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Entity : MonoBehaviour
{
    
    public Animator _animator;
    public AnimatorOverrideController overrideController;
    public AttackManager _attackManager;

    public float movementSpeed = 0.5f;

    public bool ignoreGravity = false;
    public Health health;
    public Hurtbox hurtbox;
    //Class for organizing entities, which we may or may not need.
    public List<ParticleSystem> effects = new List<ParticleSystem>();
    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        hurtbox = GetComponentInChildren<Hurtbox>();
    }

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ParticleSystem AddEffect(ParticleSystem effectPrefab)
    {
        ParticleSystem newEffect = Instantiate(effectPrefab, transform);
        effects.Add(newEffect);
        return newEffect;
    }

    public void RemoveEffect(ParticleSystem effect)
    {
        effects.Remove(effect);
        Destroy(effect.gameObject);
    }

    public int GetDirection()
    {
        return 1 * (int)Mathf.Sign(transform.localScale.x);
    }

    public virtual void GetHit(AttackObject attack)
    {
        health.LoseHealth(attack.damage);
        //health -= attack.damage;

         
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

}
