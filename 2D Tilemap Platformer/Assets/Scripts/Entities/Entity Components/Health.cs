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
    public CharacterEntity entity;

    public HealthBarUI healthbar = null;

    // Start is called before the first frame update
    void Awake()
    {
        entity = GetComponent<CharacterEntity>();    
    }

    public void SetHealthBar(HealthBarUI hBar)
    {
        healthbar = hBar;

        if(healthbar != null)
        {
            healthbar.SetHealth(currentHealth, maxHealth);
        }
    }

    public virtual void SetBaseHealth()
    {

    }

    public bool AtFullHealth()
    {
        return currentHealth == maxHealth;
    }

    public void UpdateHealth()
    {
        if(entity == null)
        {
            entity = GetComponent<CharacterEntity>();
        }
        if (entity.stats == null)
        {
            return;
        }

        int baseHealth = (int)entity.stats.GetSecondaryStat(SecondaryStatType.BaseHealth).GetValue();

        SetMaxHealth(baseHealth);
    }

    public virtual void SetMaxHealth(int value)
    {
        int difference = value - maxHealth;
        maxHealth = value;
        SetHealth(currentHealth + difference);
    }

    public virtual void SetHealth(int value)
    {
        int difference = value - currentHealth;

        currentHealth += difference;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if(healthbar != null)
        {
            healthbar.SetHealth(currentHealth, maxHealth);
        }
    }

    //Returns true if it kills the enemy
    public void LoseHealth(int Damage)
    {
        if (Damage <= 0)
        {
            return;
        }
        Color textColor = Color.white;

        if(entity is PlayerController)
        {
            textColor = Color.red;
        } 


        entity.ShowFloatingText(Damage.ToString(), textColor);
        

        SetHealth(currentHealth - Damage);

        if (!entity.isDead && currentHealth <= 0)
        {

            entity.Die();
        }

    }

    //Returns true if it kills the enemy
    public void LoseHealth(AttackHitData hitData)
    {


        if (hitData.damageDealt <= 0)
        {
            return;
        }
        Color textColor = Color.white;

        if (entity is PlayerController)
        {
            textColor = Color.red;
        }

        if (hitData.crit)
        {
            textColor = Color.yellow;
            entity.ShowFloatingText(hitData.damageDealt.ToString(), textColor, 1, 1, 2);
        }
        else
        {
            entity.ShowFloatingText(hitData.damageDealt.ToString(), textColor);
        }

        SetHealth(currentHealth - hitData.damageDealt);

        /*
        foreach (Ability ability in entity.abilities)
        {
            ability.OnHurt(hitData);
        }
        */

        if (!entity.isDead && currentHealth <= 0)
        {
            foreach (Ability ability in hitData.attackOwner.abilities)
            {
                ability.OnKill(hitData);
            }

            hitData.attackOwner.OnKill(entity);


            entity.Die();
        }

    }

    public virtual void GainLife(int Heals)
    {
        if(Heals < 0)
        {
            Heals = 0;
        }

        entity.ShowFloatingText(Heals.ToString(), Color.green);

        SetHealth(currentHealth + Heals);

    }

}
