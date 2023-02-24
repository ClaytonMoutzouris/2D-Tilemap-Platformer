using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : Entity, IHurtable
{
    public Hurtbox hurtbox;
    public ParticleSystem prefab;

    protected override void Awake()
    {
        base.Awake();

        hurtbox.SetOwner(this);
    }

    void Update()
    {
        if (_controller.isGrounded)
        {
            _controller.velocity.x = (Mathf.Pow((1 - GambleConstants.GROUND_FRICTION), Time.deltaTime)) * _controller.velocity.x;
        }

        _controller.move();
    }

    public bool CheckFriendly(Entity entity)
    {
        return false;
    }

    public bool CheckHit(AttackObject attackObject)
    {
        return true;
    }

    public Health GetHealth()
    {
        return null;
    }

    public void GetHurt(ref AttackHitData hitData)
    {
        hitData.killedEnemy = true;
        BreakObject();

    }

    public Hurtbox GetHurtbox()
    {
        return hurtbox;
    }

    public void BreakObject()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
