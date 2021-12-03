using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTriggerObject : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public LayerMask triggerMask;
    public BoxCollider2D _triggerBox;

    //public GameObject itemTooltip;

    // Start is called before the first frame update
    void Start()
    {

    }

    protected void Awake()
    {
        _triggerBox = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Trigger()
    {

    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(triggerMask.Contains(collider.gameObject.layer))
        {
            Trigger();
        }

    }


    public void OnTriggerStay2D(Collider2D col)
    {



    }


    public void OnTriggerExit2D(Collider2D col)
    {


    }

}
