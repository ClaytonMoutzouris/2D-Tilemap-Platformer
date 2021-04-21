using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Entity : MonoBehaviour
{
    
    public Animator _animator;
    public AnimatorOverrideController overrideController;

    //Class for organizing entities, which we may or may not need.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = overrideController;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
