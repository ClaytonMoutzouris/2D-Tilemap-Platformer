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

    //Effected is where the projectile is spawning, the owner is who owns it
    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(owner, effected, data);


        Projectile proj = Instantiate(projectile.projectileBase, effected.transform.position, Quaternion.identity);

        if(proj != null)
        {
            //Entity 

            direction.x *= effected.GetDirection();
            

            proj.SetData(projectile);
            proj._attackObject.attackData = projectileAttackData;

            proj._attackObject.SetOwner(owner);
            

            proj.SetDirection(direction);
        }

    }
}
