using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleChain : Chain
{
    public ParticleSystem system;
    public float distance;

    protected override void UpdateTransformForScale()
    {
        if (startObject)
        {
            startPos = startObject.transform.position;
        }

        if (endObject)
        {
            endPos = endObject.transform.position;

        }

        distance = Vector3.Distance(startPos, endPos);
        //spriteRenderer.size = new Vector2(.25f, distance);

        var main = system.main;
        main.startLifetime = (distance/main.startSpeed.constant);

        Vector3 middlePoint = (startPos + endPos) / 2;
        transform.position = startPos;

        Vector3 rotationDirection = (endPos - startPos);
        transform.up = rotationDirection;
    }
}
