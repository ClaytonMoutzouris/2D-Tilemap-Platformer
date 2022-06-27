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

    #region MoveToController
    public Vector3 _velocity = Vector3.zero;
    public bool ignoreGravity = false;
    #endregion
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
        _velocity = _controller.velocity;


        if (!ignoreGravity)
            _velocity.y += GambleConstants.GRAVITY * Time.deltaTime;

        _controller.move(_velocity * Time.deltaTime);

        _velocity = _controller.velocity;

        if (_controller.isGrounded)
        {
            _velocity.x = 0;
            _velocity.y = 0;

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
