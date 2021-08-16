using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sightbox : MonoBehaviour
{
    public CircleCollider2D sightCollider;
    public ContactFilter2D contactFilter;
    public ColliderState state = ColliderState.Open;
    public List<Entity> inSight = new List<Entity>();
    public EnemyEntity owner;

    // Start is called before the first frame update
    void Start()
    {
        sightCollider = GetComponent<CircleCollider2D>();
        owner = GetComponentInParent<EnemyEntity>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == ColliderState.Closed) { return; }

        //Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + (Vector3)hitbox.offset, hitbox.size, transform.rotation.z, mask);
        inSight.Clear();

        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(sightCollider, contactFilter, colliders);

        for (int i = 0; i < colliders.Count; i++)
        {
            Entity entity = colliders[i].GetComponent<Entity>();
            if(!entity.health.IsDead())
            {
                inSight.Add(entity);
            }
        }

        state = colliders.Count > 0 ? ColliderState.Colliding : ColliderState.Open;

    }

}
