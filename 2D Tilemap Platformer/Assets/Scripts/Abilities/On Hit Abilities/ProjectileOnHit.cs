using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileOnHit", menuName = "ScriptableObjects/Abilities/ProjectileOnHit")]
public class ProjectileOnHit : Ability
{
    /**
     * TODO: There is a problem here where this can trigger itself
     */
    //This 


    public ProjectileData proj;
    //Could probably include more stuff here
    //Such as how the projectile is aimed/give it different attack data etc.

    public override void OnHit(AttackHitData hitData)
    {
        Projectile newProj = Instantiate(proj.projectileBase, owner.transform.position, Quaternion.identity);
        newProj.SetData(proj);

        //Commenting this out causes nullref exception
        newProj._attackObject.SetOwner(owner);

        newProj.SetDirection(owner.GetDirection() * Vector2.right);
    }
}