using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sightbox : MonoBehaviour
{
    public CircleCollider2D sightCollider;

    // Start is called before the first frame update
    void Start()
    {
        sightCollider = GetComponent<CircleCollider2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
