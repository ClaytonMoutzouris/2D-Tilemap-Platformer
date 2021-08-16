using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatusEffects
{

    public enum StatusEffect { Knockback };


    public static IEnumerator Knockback(Entity entity, AttackObject attack)
    {
        float knockbackTimestamp = Time.time;
        Projectile proj = attack.GetComponent<Projectile>();
        Vector2 difference;

        if (proj)
        {
            difference = (entity.transform.position - attack.transform.position).normalized;
        }
        else
        {
            difference = (entity.transform.position - attack.owner.transform.position).normalized;
        }


        entity._velocity = difference * attack.GetAttackData().knockbackPower + Vector2.up * Mathf.Sqrt(-GambleConstants.GRAVITY) * Mathf.Clamp01(attack.GetAttackData().knockbackPower);

        entity.knockedBack = true;

        yield return new WaitForSeconds(attack.GetAttackData().knockbackPower * 0.1f);

        entity.knockedBack = false;


    }

    public static IEnumerator Poisoned(Entity entity)
    {


        //yield return new WaitForSeconds(attack.knockbackPower * 0.1f);

        yield return null;

    }

}
