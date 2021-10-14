using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnProjectileEffect", menuName = "ScriptableObjects/Effects/SpawnEffects/SpawnProjectileEffect")]
public class SpawnProjectileEffect : ImmediateEffect
{
    public ProjectileData projectile;
    public AttackData attackData;

    public override void Apply(Entity effected, Entity effector = null)
    {
        base.Apply(effected, effector);

        Projectile proj = Instantiate(projectile.projectileBase, effected.transform.position, Quaternion.identity);
        proj.SetData(projectile);

        proj._attackObject.SetOwner(effected);

        proj._attackObject.attackData = attackData;

        proj.SetDirection(effected.GetDirection() * Vector2.right);
    }

}
