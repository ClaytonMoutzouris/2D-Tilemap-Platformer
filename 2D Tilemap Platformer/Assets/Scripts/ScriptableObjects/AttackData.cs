using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "ScriptableObjects/Attacks/AttackData")]
public class AttackData : ScriptableObject
{
    public Attack baseAttack;

    public float attackSpeed;
    public Color color;
}
