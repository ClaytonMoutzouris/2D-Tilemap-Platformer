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

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public virtual void SetBaseHealth()
    {

    }

    public void UpdateHealth()
    {
        if(entity.stats == null)
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
    }

    //Returns true if it kills the enemy
    public virtual void LoseHealth(int Damage, Entity attacker = null, bool crit = false)
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

        if(crit)
        {
            textColor = Color.yellow;
            entity.ShowFloatingText(Damage.ToString(), textColor, 1, 1, 2);
        }
        else
        {
            entity.ShowFloatingText(Damage.ToString(), textColor);
        }

        currentHealth -= Damage;    
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if(attacker != null)
        {
            foreach (Ability ability in entity.abilities)
            {
                if (ability is EffectOnHurt onHurt)
                {
                    onHurt.OnHurt(attacker);
                }
            }
        }


        if (currentHealth <= 0)
        {
            //Move this somewhere better
            if (attacker != null)
            {
                foreach (Ability ability in attacker.abilities)
                {
                    if (ability is EffectOnKill onKill)
                    {
                        onKill.OnKill(entity);
                    }
                }
            }

            if(!entity.isDead)
            {
                entity.Die();
            }
        }

    }

    public virtual void GainLife(int Heals)
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
