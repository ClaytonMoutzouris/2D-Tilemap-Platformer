using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TilemapParallax : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.3f;
    [SerializeField] GameObject viewTarget;

    Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        float newXPos = viewTarget.transform.position.x * scrollSpeed;
        float newYPos = viewTarget.transform.position.y * scrollSpeed;

        tilemap.transform.position = new Vector3(newXPos, newYPos, tilemap.transform.position.z);
        
    }
}
