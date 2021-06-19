using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsBody2D))]
[RequireComponent(typeof(AttackObject))]
public class Projectile : MonoBehaviour
{
    public bool pierce = false;
    public bool boomerang = false;
    public bool homing = false;
    public bool ignoreGround = false;

    public Vector2 direction;
    public float lifeTime = 1;
    public float startTime = 0;

    public AttackObject _attackObject;

    //Physics things
    protected PhysicsBody2D _controller;
    public float projSpeed = 5;
    //Might want to create a seperate projectile class for "boomerangs", but modularity has its merits aswell
    public float elasticity = -0.5f;

    public void Awake()
    {
        _controller = GetComponent<PhysicsBody2D>();
        _attackObject = GetComponent<AttackObject>();

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

    protected void Update()
    {

        Vector3 velocity = direction * projSpeed;

        if (boomerang)
        {
            Boomerang(ref velocity);
        }

        _controller.move(velocity * Time.deltaTime);

        if(_controller.collisionState.hasCollision() && !ignoreGround)
        {
            Destroy(gameObject);
        }

        if (startTime + lifeTime <= Time.time)
        {
            Destroy(gameObject);
        }

        //Destroy the object when it makes a collision unless it has piercing
        if(!pierce && _attackObject.hits.Count > 0)
        {
            Destroy(gameObject);
        }

    }

    public void Boomerang(ref Vector3 vel)
    {
        vel = vel - vel * elasticity * (Time.time - startTime);
    }

}
