using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControllableProjectile : Projectile
{
    Entity controllingEntity;

    public override void HandleMovement()
    {
        if(_attackObject.owner is PlayerController player)
        {
            Vector2 aim = player._input.GetRightStickAim().normalized;

            if (aim != Vector2.zero)
            {
                _controller.velocity.x = aim.x * projectileData.projSpeed;
                _controller.velocity.y = aim.y * projectileData.projSpeed;
            } else
            {

            }

        }

        _controller.move();


        if (startTime + projectileData.lifeTime <= Time.time)
        {
            DestroyProjectile();
        }

        //Destroy the object when it makes a collision unless it has piercing
        if (!projectileData.projectileFlags.GetFlag(ProjectileFlagType.Piercing).GetValue() && _attackObject.hits.Count > 0)
        {
            DestroyProjectile();
        }
    }



}
