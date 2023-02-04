using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingRock : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public PhysicsBody2D _controller;
    //public GameObject itemTooltip;
    //public PressurePlate trigger;
    public float MovementSpeed = 3;

    bool spawned = false;
    public EntityDirection direction;
    RockSpawnNode spawner;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetSpawner(RockSpawnNode spawnNode)
    {
        spawner = spawnNode;
        direction = spawner.direction;
    }

    protected void Awake()
    {
        _controller = GetComponent<PhysicsBody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_controller.collisionState.groundRight || _controller.collisionState.groundLeft || _controller.velocity == Vector3.zero)
        {
            //Animation.play Destroy animation
            spawner.RockDestroyed();
            Destroy(gameObject);
        }

        _controller.velocity.x = MovementSpeed * (int)direction;

        _controller.move();

    }

    public void SetInitialDirection(Vector2 direction)
    {

    }
}
