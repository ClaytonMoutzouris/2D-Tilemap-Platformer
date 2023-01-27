using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileOnHurt", menuName = "ScriptableObjects/Abilities/ProjectileOnHurt")]
public class ProjectileOnHurt : Ability
{
    public ProjectileData proj;
    //Could probably include more stuff here
    //Such as how the projectile is aimed/give it different attack data etc.

    public override void OnHurt(AttackHitData hitData)
    {
        Projectile newProj = Instantiate(proj.projectileBase, owner.transform.position, Quaternion.identity);
        newProj.SetData(proj);
        newProj._attackObject.SetOwner(owner);

        newProj.SetDirection(owner.GetDirection() * Vector2.right);
    }
}