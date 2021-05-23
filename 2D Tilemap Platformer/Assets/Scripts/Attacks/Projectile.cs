using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : AttackObject
{
    public bool pierce = false;
    public bool boomerang = false;
    public bool homing = false;
    protected PhysicsBody2D _controller;

    public Vector2 direction;

    //Physics things
    public float projSpeed = 15;
    public bool IgnoreGravity = false;
    public float startTime = 0;
    public float elasticity = -0.5f;

    private void Awake()
    {
        _controller = GetComponent<PhysicsBody2D>();

        // Might need this later?
        /*
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
        */
    }

    private void Start()
    {
        startTime = Time.time;
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, -angle);
    }

    protected override void Update()
    {

        base.Update();

        Vector3 velocity = direction * projSpeed;

        if (boomerang)
        {
            Boomerang(ref velocity);
        }

        _controller.move(velocity * Time.deltaTime);

        if(startTime + duration <= Time.time)
        {
            Destroy(gameObject);
        }
    }

    public void Boomerang(ref Vector3 vel)
    {
        vel = vel - vel * elasticity * (Time.time - startTime);
    }

    public override void CollisionedWith(Collider2D collider)
    {
        if (hits.Contains(collider))
        {
            return;
        }

        base.CollisionedWith(collider);

        //Little extra for projectiles
        if(!pierce)
        {
            Destroy(gameObject);
        }
    }


}
