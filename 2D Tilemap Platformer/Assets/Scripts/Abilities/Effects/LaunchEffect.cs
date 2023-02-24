using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LaunchEffect", menuName = "ScriptableObjects/Effects/LaunchEffect")]
public class LaunchEffect : Effect
{
    public Vector2 launchDirection = Vector2.one;
    public float launchPower = 10;
    public bool launchAway = true;


    float timeStamp;

    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(owner, effected, data);

        if(!effectedEntity._controller.isKinematic)
        {
            if(launchAway)
            {
                Vector2 diff = effected.transform.position - owner.transform.position;
                launchDirection = diff.normalized;
            }

            Vector2 launchVector = new Vector2(launchPower * launchDirection.normalized.x, Mathf.Sqrt(launchPower * launchDirection.normalized.y * -GambleConstants.GRAVITY));

            effectedEntity._controller.velocity = launchVector;
            effectedEntity._controller.Launch(launchVector);
        }
    }

    public override void RemoveEffect()
    {
        base.RemoveEffect();
    }

    /*
    public IEnumerator HandleEffect()
    {

        
    }
    */
}
