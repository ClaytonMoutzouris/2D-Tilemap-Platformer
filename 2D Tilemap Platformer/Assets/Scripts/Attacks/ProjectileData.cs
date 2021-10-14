using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Projectile", menuName = "ScriptableObjects/Attacks/Projectiles/Projectile")]
//class for handling projectile prototype info
public class ProjectileData : ScriptableObject
{
    public Sprite image;
    public RuntimeAnimatorController animator;
    public Vector2 size;

    [Header("Flags")]
    public bool pierce = false;
    public bool boomerang = false;
    public bool homing = false;
    public bool destroyOnGround = false;
    public bool ignoreGravity = true;
    public bool ignoreGround = true;
    public bool isAngled = false;
    public bool bouncy = false;

    public float projSpeed = 5;
    public float elasticity = -0.5f;

    public float lifeTime = 1;

    //This is like the prefab for the projectile... not sure how i want to use this exactly
    public AttackData attackData;
    public ContactFilter2D contactFilter;
    public Projectile projectileBase;
    public List<ParticleSystem> visualEffects;
    //Method for taking a weapon or entities stats into account?

    [HideInInspector]
    public ProjectileFlags projectileFlags = new ProjectileFlags();
    private bool baseFlagsLoaded = false;

    //This is like a sort of init method
    public void SetBaseFlags()
    {
        if (baseFlagsLoaded)
        {
            Debug.Log("Base flags already loaded");
            return;
        }

        projectileFlags.SetFlag(new ProjectileFlag(ProjectileFlagType.Piercing, pierce));
        projectileFlags.SetFlag(new ProjectileFlag(ProjectileFlagType.Boomerang, boomerang));
        projectileFlags.SetFlag(new ProjectileFlag(ProjectileFlagType.Homing, homing));
        projectileFlags.SetFlag(new ProjectileFlag(ProjectileFlagType.DestroyOnGround, destroyOnGround));
        projectileFlags.SetFlag(new ProjectileFlag(ProjectileFlagType.IgnoreGround, ignoreGround));
        projectileFlags.SetFlag(new ProjectileFlag(ProjectileFlagType.IgnoreGravity, ignoreGravity));
        projectileFlags.SetFlag(new ProjectileFlag(ProjectileFlagType.IsAngled, isAngled));
        projectileFlags.SetFlag(new ProjectileFlag(ProjectileFlagType.Bounce, bouncy));


        baseFlagsLoaded = true;
    }
}
