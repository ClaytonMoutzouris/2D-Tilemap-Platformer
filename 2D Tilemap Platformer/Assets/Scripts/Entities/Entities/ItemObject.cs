using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public ItemData item;
    public PhysicsBody2D _controller;
    ItemSpawnNode spawner;
    Vector3 _velocity = Vector3.zero;

    public bool ignoreGravity = false;
    //public GameObject itemTooltip;

    // Start is called before the first frame update
    void Start()
    {
        if(item != null)
        {
            SetItem(Instantiate(item));
        }
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

            if (!ignoreGravity)
                _velocity.y += GambleConstants.GRAVITY * Time.deltaTime;

            _controller.move(_velocity * Time.deltaTime);

    }


    public void SetItem(ItemData item)
    {
        if(item == null)
        {
            return;
        }

        this.item = item;
        spriteRenderer.sprite = item.sprite;
        spriteRenderer.color = item.color;
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
