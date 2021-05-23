using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    public bool isActive = false;
    public List<AttackObject> objectPrototypes;
    public AnimationClip ownerAnimation;
    public float attackSpeed = 1;
    public Entity entity;
    //public List<AttackObject> activeObjects;

    public void SetObject(AttackObject attackObject)
    {
        if (attackObject == null)
            return;
        //objectPrototypes[0] = attackObject;
    }

    //A basic attack.
    public virtual IEnumerator Activate(Entity user)
    {

        yield return null;

    }

    public virtual void StartUp()
    {
        isActive = true;

    }

    public virtual void CleanUp()
    {
        isActive = false;
        Destroy(gameObject);

    }

}
