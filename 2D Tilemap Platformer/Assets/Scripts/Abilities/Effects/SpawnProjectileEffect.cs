using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnProjectile", menuName = "ScriptableObjects/Effects/SpawnProjectile")]
public class SpawnProjectileEffect : Effect
{
    public ProjectileData projectile;
    public AttackData projectileAttackData;
    public float projectileSpeed = 3;
    public float lifeTime = 5;

    public Vector2 direction = Vector2.right;

    public override void ApplyEffect(Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(effected, data);


        Projectile proj = Instantiate(projectile.projectileBase, effected.transform.position, Quaternion.identity);

        if(proj != null)
        {
            //Entity 

            direction.x *= effected.GetDirection();
            

            proj.SetData(projectile);
            proj._attackObject.attackData = projectileAttackData;

            if(effected is Projectile projEntity)
            {
                proj._attackObject.SetOwner(projEntity._attackObject.owner);
            }
            else
            {
                proj._attackObject.SetOwner(effected);
            }

            proj.SetDirection(direction);
        }

    }
}
