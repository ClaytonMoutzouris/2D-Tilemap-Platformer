﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxObject : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public PhysicsBody2D _controller;

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

        _controller.move();

    }


}
