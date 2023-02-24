using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "ScriptableObjects/PowerUps/PowerUp")]
public class PowerUp : ScriptableObject
{
    [Header("Powerup Info")]
    public Sprite sprite;
    public Color color;

    public Effect powerUpEffect;
}