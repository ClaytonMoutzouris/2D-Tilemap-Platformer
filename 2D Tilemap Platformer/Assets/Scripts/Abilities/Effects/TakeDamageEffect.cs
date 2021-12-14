using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TakeDamage", menuName = "ScriptableObjects/Effects/TakeDamage")]
public class TakeDamageEffect : Effect
{
    public int baseDamage = 1;


    public override void ApplyEffect()
    {
        effected.health.LoseHealth(baseDamage);
    }
}
