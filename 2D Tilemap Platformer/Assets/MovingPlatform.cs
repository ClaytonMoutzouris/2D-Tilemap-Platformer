using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovingPlatformMovement { Horizontal, Vertical, Clockwise, Counterclockwise };
public class MovingPlatform : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public PhysicsBody2D _controller;

    public MovingPlatformMovement movementType = MovingPlatformMovement.Horizontal;

    public float MovementSpeed = 2;
    public Vector3 baseDirection = Vector2.right;
    
    //public GameObject itemTooltip;

    // Start is called before the first frame update
    void Start()
    {
        switch(movementType)
        {
            case MovingPlatformMovement.Horizontal:
                baseDirection = Vector2.right;
                break;
            case MovingPlatformMovement.Clockwise:
                baseDirection = Vector2.right;
                break;
            case MovingPlatformMovement.Vertical:
                baseDirection = Vector2.up;
                break;
            case MovingPlatformMovement.Counterclockwise:
                baseDirection = Vector2.left;

                break;
        }

        _controller.velocity = MovementSpeed*baseDirection;


    }

    protected virtual void Awake()
    {
        _controller = GetComponent<PhysicsBody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }



    // Update is called once per frame
    void Update()
    {
        //_controller.collisionState.

        switch (movementType)
        {
            case MovingPlatformMovement.Horizontal:
                MoveHorizontal();

                break;
            case MovingPlatformMovement.Clockwise:
                MoveClockwise();
                break;
            case MovingPlatformMovement.Vertical:
                MoveVertical();
                break;
            case MovingPlatformMovement.Counterclockwise:
                MoveCounterclockwise();
                break;
        }

        _controller.move();

    }

    public virtual void MoveHorizontal()
    {
        if (_controller.collisionState.groundRight)
        {
            baseDirection = Vector2.left;
        }
        else if (_controller.collisionState.groundLeft)
        {
            baseDirection = Vector2.right;
        }

        _controller.velocity = MovementSpeed * baseDirection;
    }

    public virtual void MoveVertical()
    {
        if (_controller.collisionState.groundBelow)
        {
            baseDirection = Vector2.up;
        }
        else if (_controller.collisionState.groundAbove)
        {
            baseDirection = Vector2.down;
        }

        _controller.velocity = MovementSpeed * baseDirection;

    }

    public void MoveClockwise()
    {
        if (_controller.collisionState.groundRight && !_controller.collisionState.groundBelow)
        {
            baseDirection = Vector2.down;
        }
        else if (_controller.collisionState.groundBelow && !_controller.collisionState.groundLeft)
        {
            baseDirection = Vector2.left;
        }
        else if (_controller.collisionState.groundLeft && !_controller.collisionState.groundAbove)
        {
            baseDirection = Vector2.up;
        }
        else if (_controller.collisionState.groundAbove && !_controller.collisionState.groundRight)
        {
            baseDirection = Vector2.right;
        }

        _controller.velocity = MovementSpeed * baseDirection;

    }

    public void MoveCounterclockwise()
    {
        if (_controller.collisionState.groundLeft && !_controller.collisionState.groundBelow)
        {
            baseDirection = Vector2.down;
        }
        else if (_controller.collisionState.groundAbove && !_controller.collisionState.groundLeft)
        {
            baseDirection = Vector2.left;
        }
        else if (_controller.collisionState.groundRight && !_controller.collisionState.groundAbove)
        {
            baseDirection = Vector2.up;
        }
        else if (_controller.collisionState.groundBelow && !_controller.collisionState.groundRight)
        {
            baseDirection = Vector2.right;
        }

        _controller.velocity = MovementSpeed * baseDirection;

    }

}
