using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public GameObject startObject;
    public GameObject endObject;
    public Vector3 startPos;
    public Vector3 endPos;

    public void SetObjects(GameObject obj1, GameObject obj2)
    {
        startObject = obj1;
        endObject = obj2;
    }

    public void Update()
    {
        UpdateTransformForScale();
    }

    protected virtual void UpdateTransformForScale()
    {
        if(startObject)
        {
            startPos = startObject.transform.position;
        }

        if(endObject)
        {
            endPos = endObject.transform.position;

        }

        float distance = Vector3.Distance(startPos, endPos);
        spriteRenderer.size = new Vector2(.25f, distance);

        Vector3 middlePoint = (startPos + endPos) / 2;
        transform.position = middlePoint;

        Vector3 rotationDirection = (endPos - startPos);
        transform.up = rotationDirection;

        if(!startObject && !endObject)
        {
            Destroy(gameObject, 0.5f);
        }
    }


}
