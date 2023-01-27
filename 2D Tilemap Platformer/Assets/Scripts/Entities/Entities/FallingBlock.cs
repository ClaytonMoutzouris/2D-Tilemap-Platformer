using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour, ITriggerable
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public PhysicsBody2D _controller;
    //public GameObject itemTooltip;
    public PressurePlate trigger;
    bool spawned = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    protected void Awake()
    {
        _controller = GetComponent<PhysicsBody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void SpawnTrigger()
    {
        Vector2Int leverPosition = GameGrid.instance.GetGroundBelow((int)transform.position.x, (int)transform.position.y);
        PressurePlate newLever = Instantiate(trigger, (Vector2)leverPosition + new Vector2(0.5f, 0.5f), Quaternion.identity);
        newLever.triggerable = this;
        spawned = true;
        //GameObject temp = Instantiate(Resources.Load("Prefabs/Entities/" + tile.SpawnObject) as GameObject, new Vector3(tile.LocalPlace.x + 0.5f, tile.LocalPlace.y + 0.5f, 0), Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        if (!spawned)
        {
            SpawnTrigger();
        }

        if (_controller.isGrounded)
        {
            _controller.velocity.x = (Mathf.Pow((1 - GambleConstants.GROUND_FRICTION), Time.deltaTime)) * _controller.velocity.x;
        }


        _controller.move();

    }

    public void Trigger()
    {
        _controller.ignoreGravity = false;
    }
}
