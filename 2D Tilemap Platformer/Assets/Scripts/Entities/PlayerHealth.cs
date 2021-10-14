using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    PlayerController player;

    private void Awake()
    {
        player = GetComponent<PlayerController>();

    }

    public override void GainLife(int Heals)
    {
        base.GainLife(Heals);
    }

    public override void SetHealth(int value)
    {
        base.SetHealth(value);
    }

    public override void SetMaxHealth(int value)
    {
        base.SetMaxHealth(value);
    }



}
