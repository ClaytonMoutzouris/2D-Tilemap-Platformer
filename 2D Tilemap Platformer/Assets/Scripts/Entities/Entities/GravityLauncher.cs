using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityLauncher : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Collider2D hitbox;
    public ContactFilter2D contactFilter;
    public Vector2 launchDirection = Vector2.up;
    public float launchValue = 10;

    public void Awake()
    {
        
    }

    public void Update()
    {
        CheckCollisions();
    }

    public void SetLaunchDirection(Vector2 dir)
    {
        launchDirection = dir;
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
                //body.StartCoroutine(StatusEffects.Knockback(body.GetComponent<Entity>(), launchDirection, launchValue));
                //body.transform.Translate(deltaMovement, Space.World);
                Vector2 launchVector = new Vector2(launchValue * launchDirection.normalized.x, Mathf.Sqrt(launchValue * launchDirection.normalized.y * -GambleConstants.GRAVITY));
                Debug.Log("Launch Velocity: " + launchVector);
                body.velocity = launchVector;
                body.Launch(launchVector);

            }

        }

    }
}
