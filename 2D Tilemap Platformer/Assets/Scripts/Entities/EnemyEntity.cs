using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : Entity
{
    public float jumpHeight = 3f;

    public PhysicsBody2D _controller;


    public int normalizedHorizontalSpeed = 0;


    protected override void Awake()
    {
        base.Awake();

        _controller = GetComponent<PhysicsBody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _velocity.x = normalizedHorizontalSpeed * movementSpeed;

        if (!ignoreGravity)
            _velocity.y += GambleConstants.GRAVITY * Time.deltaTime;

        _controller.move(_velocity * Time.deltaTime);

        _velocity = _controller.velocity;

    }
}


