using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangProjectile : Projectile
{
    public float elasticity = 1;

    private void Awake()
    {

    }



    protected override void Update()
    {

        base.Update();

        Vector3 velocity = direction * projSpeed;


        _controller.move(velocity * Time.deltaTime);


    }

    public override void CollisionedWith(Collider2D collider)
    {
        if (hits.Contains(collider))
        {
            return;
        }

        base.CollisionedWith(collider);

        //Little extra for projectiles
        if(!pierce)
        {
            Destroy(gameObject);
        }
    }


}
