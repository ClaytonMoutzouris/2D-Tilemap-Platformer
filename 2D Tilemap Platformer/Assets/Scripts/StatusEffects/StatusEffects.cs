using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatusEffects
{

    public static IEnumerator Knockback(Entity entity, Vector2 direction, float knockbackPower)
    {
        if(knockbackPower <= 0)
        {
            yield break;
        }
        float knockbackTimestamp = Time.time;

        PhysicsBody2D body = entity.GetComponent<PhysicsBody2D>();
        if(body)
        {
            body.velocity = direction * knockbackPower + Vector2.up * Mathf.Sqrt(-GambleUtilities.GetGravityModifier(body)) * Mathf.Clamp01(knockbackPower);
            body.Launch(body.velocity);

        }

        entity.knockedBack = true;

        yield return new WaitForSeconds(knockbackPower * 0.1f);

        entity.knockedBack = false;


    }

}
