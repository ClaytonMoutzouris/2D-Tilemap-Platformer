using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour, ITriggerable
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public PhysicsBody2D _controller;
    Vector3 _velocity = Vector3.zero;

    public bool ignoreGravity = true;
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

        _velocity = _controller.velocity;

        if (!ignoreGravity)
            _velocity.y += GambleConstants.GRAVITY * Time.deltaTime;

        _controller.move(_velocity * Time.deltaTime);

        _velocity = _controller.velocity;
    }

    public void Trigger()
    {
        ignoreGravity = false;
    }
}
