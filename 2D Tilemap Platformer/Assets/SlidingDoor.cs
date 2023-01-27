using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour, ITriggerable
{
    public bool isOpen = false;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public PhysicsBody2D _controller;
    public List<IInteractable> interactables;
    public Lever leverPrefab;
    public bool spawned = false;

    public void Awake()
    {
        _controller = GetComponent<PhysicsBody2D>();
        animator = GetComponent<Animator>();

    }

    public void SpawnLever()
    {
        Vector2Int leverPosition = GameGrid.instance.FindGroundWithinRange((int)transform.position.x, (int)transform.position.y, 1);
        Lever newLever = Instantiate(leverPrefab, (Vector2)leverPosition + new Vector2(0.5f, 0.5f), Quaternion.identity);
        newLever.triggerable = this;
        spawned = true;
        //GameObject temp = Instantiate(Resources.Load("Prefabs/Entities/" + tile.SpawnObject) as GameObject, new Vector3(tile.LocalPlace.x + 0.5f, tile.LocalPlace.y + 0.5f, 0), Quaternion.identity);

    }

    public void Update()
    {

        //This is just for testing
        /*
        if(Time.time > 10)
        {
            if (!isOpen)
            {
                Open();
            }

        }
        */

        if(!spawned)
        {
            SpawnLever();
        }
        

    }
 

    public void Open()
    {
        if(!isOpen)
        {
            animator.Play("SlidingDoor_Opening");
            isOpen = true;
        }

    }

    public void Close()
    {
        if (isOpen)
        {
            animator.Play("SlidingDoor_Closing");
            isOpen = false;
        }
    }

    public void Trigger()
    {
        if(isOpen)
        {
            Close();
        } else
        {
            Open();
        }
    }
}
