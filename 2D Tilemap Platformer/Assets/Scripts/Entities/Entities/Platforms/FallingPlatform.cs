using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MovingPlatform
{

    public bool triggered = true;
    public float waitTime = 0.5f;

    // Update is called once per frame
    protected override void Update()
    {
        //_controller.collisionState.

        if (!triggered && _controller.riders.Count > 0)
        {
            Trigger();
        }

        _controller.move();

        if(triggered && _controller.isGrounded)
        {
            Destroy(gameObject);
        }

    }

    public void Trigger()
    {
        triggered = true;
        gameObject.layer = LayerMask.NameToLayer("OneWayPlatform");
        StartCoroutine(FallDelay());
    }

    public IEnumerator FallDelay()
    {
        float timeStamp = Time.time;

        while(Time.time < timeStamp + waitTime)
        {

            yield return null;
        }

        _controller.ignoreGravity = false;

    }

    public void Respawn()
    {

    }

}
