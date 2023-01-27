using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectileEffect : TriggeredEffect
{
    public ProjectileData projectile;
    public AttackData attackData;
    public float projectileSpeed = 3;
    public float lifeTime = 5;

    public Vector2 direction = Vector2.right;

    public override void Trigger()
    {
        base.Trigger();


        Projectile proj = Instantiate(projectile.projectileBase, effected.transform.position, Quaternion.identity);

        if(proj != null)
        {
            //Entity 

            //direction.x *= effected.GetDirection();
            

            proj.SetData(projectile);
            proj._attackObject.SetOwner(effected);
            proj._attackObject.attackData = attackData;
            proj._attackObject.attackData.owner = effected;


            proj.SetDirection(direction);
        }

    }
}
