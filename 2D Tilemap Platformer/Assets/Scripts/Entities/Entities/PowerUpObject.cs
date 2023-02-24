using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpObject : Entity
{
    public PowerUp powerUp;
    PowerUpSpawnNode spawner;

    public void SetSpawner(PowerUpSpawnNode spawnNode)
    {
        spawner = spawnNode;
    }

    public void CollectPowerUp(Entity entity)
    {
        if (entity != null && powerUp.powerUpEffect != null)
        {
            Effect effect = Instantiate(powerUp.powerUpEffect);
            effect.ApplyEffect(entity);

            if (spawner != null)
            {
                spawner.Collected();
            }
            Destroy(gameObject);

        }
    }

    public void Update()
    {


        _controller.move();

    }

    public void SetEffect(PowerUp powerUp)
    {
        if (powerUp == null)
        {
            return;
        }

        this.powerUp = powerUp;

        spriteRenderer.sprite = powerUp.sprite;
        spriteRenderer.color = powerUp.color;
    }
}
