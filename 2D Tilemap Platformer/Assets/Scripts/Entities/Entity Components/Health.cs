using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Component for tracking health, had by any entities that can take damage
 */
public class Health : MonoBehaviour
{

    public int maxHealth = 20;
    public int currentHealth = 20;
    public Entity entity;

    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<Entity>();    
    }

    public void SetMaxHealth(int value)
    {
        int difference = value - maxHealth;
        maxHealth = value;
        SetHealth(currentHealth + difference);
    }

    public void SetHealth(int value)
    {
        int difference = value - currentHealth;
        if(difference > 0)
        {
            GainLife(difference);
        } else if (difference < 0)
        {
            LoseHealth(difference);

        }
    }

    public void LoseHealth(int Damage)
    {
        if (Damage < 0)
        {
            Damage = 0;
        }

        entity.ShowFloatingText(Damage.ToString(), Color.red);

        currentHealth -= Damage;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void GainLife(int Heals)
    {
        if(Heals < 0)
        {
            Heals = 0;
        }

        entity.ShowFloatingText(Heals.ToString(), Color.green);

        currentHealth += Heals;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

    }

}
