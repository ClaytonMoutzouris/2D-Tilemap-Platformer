using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnProjectileEffect", menuName = "ScriptableObjects/Effects/SpawnEffects/SpawnProjectileEffect")]
public class SpawnProjectileEffect : ImmediateEffect
{
    public Projectile spawnPrefab;
    public AttackData attackData;

    public override void Apply(Entity effected, Entity effector = null)
    {
        base.Apply(effected, effector);

        Projectile projectile = Instantiate(spawnPrefab, effected.transform.position, Quaternion.identity);
        projectile._attackObject.SetOwner(effected);

        projectile._attackObject.attackData = attackData;

        projectile.SetDirection(effected.GetDirection() * Vector2.right);
    }

    public void Apply(GameObject spawner, Entity owner, Entity effector = null)
    {
        base.Apply(owner, effector);

        Projectile projectile = Instantiate(spawnPrefab, owner.transform.position, Quaternion.identity);
        projectile._attackObject.SetOwner(owner);

        projectile._attackObject.attackData = attackData;

        projectile.SetDirection(owner.GetDirection() * Vector2.right);
    }

}
