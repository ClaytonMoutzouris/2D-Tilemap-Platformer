using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Entity : MonoBehaviour
{
    
    public Animator _animator;
    public AnimatorOverrideController overrideController;
    public AttackManager _attackManager;
    public Vector3 _velocity;

    public float movementSpeed = 0.5f;

    public bool ignoreGravity = false;

    public MovementState movementState = MovementState.Idle;

    //Class for organizing entities, which we may or may not need.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetDirection()
    {
        return 1 * (int)Mathf.Sign(transform.localScale.x);
    }
}
