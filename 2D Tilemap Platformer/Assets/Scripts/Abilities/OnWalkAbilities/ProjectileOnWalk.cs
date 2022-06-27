using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileOnWalk", menuName = "ScriptableObjects/Abilities/ProjectileOnWalk")]
public class ProjectileOnWalk : Ability
{
    float walkCount = 0;
    Vector2 previousPosition = Vector2.zero;
    public float walkThreshold = 1;
    public ProjectileData proj;

    public override void OnWalk()
    {
        walkCount += Mathf.Abs(owner.transform.position.x - previousPosition.x);

        previousPosition = owner.transform.position;

        if (walkCount >= walkThreshold)
        {
            Projectile newProj = Instantiate(proj.projectileBase, owner.transform.position, Quaternion.identity);
            newProj.SetData(proj);
            newProj._attackObject.SetOwner(owner);

            newProj.SetDirection(owner.GetDirection() * Vector2.right);

            walkCount = 0;
        }



    }
}
