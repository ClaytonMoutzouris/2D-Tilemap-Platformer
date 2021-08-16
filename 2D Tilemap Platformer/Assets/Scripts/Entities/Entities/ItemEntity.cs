using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity : Entity
{
    public Item item;
    public PhysicsBody2D _controller;
    ItemSpawnNode spawner;

    // Start is called before the first frame update
    void Start()
    {
        if(item != null)
        {
            SetItem(Instantiate(item));
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _controller = GetComponent<PhysicsBody2D>();
    }

        // Update is called once per frame
    void Update()
    {
        
            if (!ignoreGravity)
                _velocity.y += GambleConstants.GRAVITY * Time.deltaTime;

            _controller.move(_velocity * Time.deltaTime);
        
    }


    public void SetItem(Item item)
    {
        if(item == null)
        {
            return;
        }

        this.item = item;
        spriteRenderer.sprite = item.sprite;
    }

    public void SetSpawner(ItemSpawnNode spawnNode)
    {
        spawner = spawnNode;
    }

    public void Collect()
    {
        if(spawner != null)
        {
            spawner.ItemCollected();
        }

        Destroy(gameObject);


    }

}
