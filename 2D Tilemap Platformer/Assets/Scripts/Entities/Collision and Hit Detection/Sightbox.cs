using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sightbox : MonoBehaviour
{
    public CircleCollider2D sightCollider;
    public ContactFilter2D contactFilter;
    public ColliderState state = ColliderState.Open;
    public List<Entity> entitiesInSight = new List<Entity>();
    public List<GameObject> objectsInSight = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        sightCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == ColliderState.Closed) { return; }

        //Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + (Vector3)hitbox.offset, hitbox.size, transform.rotation.z, mask);
        entitiesInSight.Clear();
        objectsInSight.Clear();

        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(sightCollider, contactFilter, colliders);

        for (int i = 0; i < colliders.Count; i++)
        {
            GameObject objectInSight = colliders[i].GetComponent<GameObject>();

            objectsInSight.Add(objectInSight);

            Entity entity = colliders[i].GetComponent<Entity>();
            if (entity)
            {
                if(!entity.health.IsDead())
                {
                    entitiesInSight.Add(entity);
                }
            }



        }

        state = colliders.Count > 0 ? ColliderState.Colliding : ColliderState.Open;

    }

    public void SetRadius(float radius)
    {
        sightCollider.radius = radius;
    }

}
