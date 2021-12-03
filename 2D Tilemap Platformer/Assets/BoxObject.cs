using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxObject : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public PhysicsBody2D _controller;
    Vector3 _velocity = Vector3.zero;

    public bool ignoreGravity = false;
    //public GameObject itemTooltip;

    // Start is called before the first frame update
    void Start() {

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


}
