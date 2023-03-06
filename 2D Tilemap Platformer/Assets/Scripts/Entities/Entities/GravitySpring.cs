using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySpring : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Collider2D hitbox;
    public ContactFilter2D contactFilter;

    public void Awake()
    {
        
    }

    public void Update()
    {
        CheckCollisions();
    }

    public void CheckCollisions()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(hitbox, contactFilter, colliders);

        for (int i = 0; i < colliders.Count; i++)
        {

            PhysicsBody2D body = colliders[i].GetComponent<PhysicsBody2D>();
            if(body)
            {

                //body.move(deltaMovement);
                if(body.velocity.y < -2.0f)
                {
                    body.velocity.y = -2.0f;
                }
                body.velocity.y += -GambleUtilities.GetGravityModifier(body) * 1.5f * Time.deltaTime;
                //body.transform.Translate(deltaMovement, Space.World);

            }

        }

    }
}
