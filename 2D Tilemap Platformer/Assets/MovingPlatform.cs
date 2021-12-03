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

    Vector3 _velocity = Vector3.zero;

    public float MovementSpeed = 2;
    
    //public GameObject itemTooltip;

    // Start is called before the first frame update
    void Start()
    {
        switch(movementType)
        {
            case MovingPlatformMovement.Horizontal:
                _velocity.x = MovementSpeed;
                break;
            case MovingPlatformMovement.Clockwise:
                _velocity.x = MovementSpeed;
                break;
            case MovingPlatformMovement.Vertical:
                _velocity.y = MovementSpeed;
                break;
            case MovingPlatformMovement.Counterclockwise:
                _velocity.x = -MovementSpeed;
                break;
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

        _controller.move(_velocity * Time.deltaTime);

    }

    public void MoveHorizontal()
    {
        if (_controller.collisionState.groundRight)
        {
            _velocity.y = 0;
            _velocity.x = -MovementSpeed;
        }
        else if (_controller.collisionState.groundLeft)
        {
            _velocity.y = 0;
            _velocity.x = MovementSpeed;
        }
    }

    public void MoveVertical()
    {
        if (_controller.collisionState.groundBelow)
        {
            _velocity.y = MovementSpeed;
            _velocity.x = 0;
        }
        else if (_controller.collisionState.groundAbove)
        {
            _velocity.y = -MovementSpeed;
            _velocity.x = 0;
        }
    }

    public void MoveClockwise()
    {
        if (_controller.collisionState.groundRight && !_controller.collisionState.groundBelow)
        {
            _velocity.y = -MovementSpeed;
            _velocity.x = 0;
        }
        else if (_controller.collisionState.groundBelow && !_controller.collisionState.groundLeft)
        {
            _velocity.x = -MovementSpeed;
            _velocity.y = 0;
        }
        else if (_controller.collisionState.groundLeft && !_controller.collisionState.groundAbove)
        {
            _velocity.y = MovementSpeed;
            _velocity.x = 0;
        }
        else if (_controller.collisionState.groundAbove && !_controller.collisionState.groundRight)
        {
            _velocity.x = MovementSpeed;
            _velocity.y = 0;
        }
    }

    public void MoveCounterclockwise()
    {
        if (_controller.collisionState.groundLeft && !_controller.collisionState.groundBelow)
        {
            _velocity.y = -MovementSpeed;
            _velocity.x = 0;
        }
        else if (_controller.collisionState.groundAbove && !_controller.collisionState.groundLeft)
        {
            _velocity.x = -MovementSpeed;
            _velocity.y = 0;
        }
        else if (_controller.collisionState.groundRight && !_controller.collisionState.groundAbove)
        {
            _velocity.y = MovementSpeed;
            _velocity.x = 0;
        }
        else if (_controller.collisionState.groundBelow && !_controller.collisionState.groundRight)
        {
            _velocity.x = MovementSpeed;
            _velocity.y = 0;
        }
    }

}
