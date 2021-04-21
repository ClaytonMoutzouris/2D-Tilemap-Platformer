using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : AttackObject
{
    public bool pierce = false;
    private PhysicsBody2D _controller;

    public Vector2 direction;

    //Physics things
    public float projSpeed = 15;
    public bool IgnoreGravity = false;


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


        _controller.move(velocity * Time.deltaTime);


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
