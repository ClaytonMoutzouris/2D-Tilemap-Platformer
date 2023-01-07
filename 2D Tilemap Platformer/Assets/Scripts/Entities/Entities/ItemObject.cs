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

    //public GameObject itemTooltip;

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

        // Update is called once per frame
    void Update()
    {


        _controller.move();


        if (_controller.isGrounded)
        {
            _controller.velocity.x = 0;
            _controller.velocity.y = 0;

        }
    }

    public IEnumerator Despawn(float despawnTime)
    {

        yield return new WaitForSeconds(despawnTime);

        Destroy(gameObject);


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
