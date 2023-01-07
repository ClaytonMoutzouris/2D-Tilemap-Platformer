using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpObject : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public PowerUp powerUp;
    PowerUpSpawnNode spawner;

    public PhysicsBody2D _controller;

    void Awake()
    {
        _controller = GetComponent<PhysicsBody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }


    public void SetSpawner(PowerUpSpawnNode spawnNode)
    {
        spawner = spawnNode;
    }

    public void CollectPowerUp(Entity entity)
    {
        if (entity != null && powerUp.powerUpEffect != null)
        {
            StatusEffect effect = Instantiate(powerUp.powerUpEffect);
            effect.ApplyEffect(entity);

            if (spawner != null)
            {
                spawner.Collected();
            }
            Destroy(gameObject);

        }
    }

    public void CheckCollisions()
    {

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
