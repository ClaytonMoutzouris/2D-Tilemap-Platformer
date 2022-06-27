using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColliderState
{

    Closed,

    Open,

    Colliding

}

public class Hitbox : MonoBehaviour
{
    public LayerMask mask;

    public Color inactiveColor;
    public Color collisionOpenColor;
    public Color collidingColor;

    private IHitboxResponder _responder = null;

    public BoxCollider2D boxCollider;

    public ColliderState _state;


    public void Enable()
    {
        _state = ColliderState.Open;

    }

    public void Disable()
    {
        _state = ColliderState.Closed;

    }
}