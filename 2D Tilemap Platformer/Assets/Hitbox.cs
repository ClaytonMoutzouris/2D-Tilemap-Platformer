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


    private ColliderState _state;


    /*

        and your methods

    */
    /*
    private void Update()
    {
        if (_state == ColliderState.Closed) { return; }

        Collider[] colliders = Physics.OverlapBox(transform.position, hitboxSize, transform.rotation, mask);


        if (colliders.Length > 0)
        {

            _state = ColliderState.Colliding;

            // We should do something with the colliders

        }
        else
        {

            _state = ColliderState.Open;

        }


    }

    public void HitboxUpdate()
    {
        if (_state == ColliderState.Closed) { return; }

        Collider[] colliders = Physics.OverlapBox(position, boxSize, rotation, mask);


        for (int i = 0; i < colliders.Length; i++)
        {

            Collider aCollider = colliders[i];

            _responder?.collisionedWith(aCollider);

        }


        _state = colliders.Length > 0 ? ColliderState.Colliding : ColliderState.Open;


    }

    public void useResponder(IHitboxResponder responder)
    {
        _responder = responder;

    }

    */

    public void Enable()
    {
        _state = ColliderState.Open;

    }

    public void Disable()
    {
        _state = ColliderState.Closed;

    }
}