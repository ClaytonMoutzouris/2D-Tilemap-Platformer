﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IHurtable, IInteractable
{
    public bool isOpened = false;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Hurtbox hurtbox;

    public List<ItemData> lootTable;
    public ItemObject itemPrefab;
    ChestSpawnNode spawner;

    public void Awake()
    {
        hurtbox = GetComponentInChildren<Hurtbox>();
        hurtbox.SetOwner(this);
    }

    public void GetHurt(AttackObject attackObject)
    {
        //Trigger();
        Interact(attackObject.owner);
    }

    public void Interact(Entity entity)
    {

        if(isOpened)
        {
            return;
        }

        isOpened = true;
        animator.Play("Chest_Open");

        Vector2 dir = Random.Range(-0.5f, 0.5f) * Vector2.right + Vector2.up;

        ItemObject dropped = Instantiate(itemPrefab);
        dropped.transform.position = transform.position;
        ItemData newData = Instantiate(lootTable[Random.Range(0, lootTable.Count)]);
        newData.RandomizeStats();

        dropped.SetItem(newData);

        dropped._velocity = dir * 4;

        if(spawner != null)
        {
            spawner.ChestOpened();
        }

        StartCoroutine(Despawn(4));
    }

    public IEnumerator Despawn(float despawnTime)
    {

        yield return new WaitForSeconds(despawnTime);

        Destroy(gameObject);


    }

    public void SetSpawner(ChestSpawnNode spawnNode)
    {
        spawner = spawnNode;
    }
}
