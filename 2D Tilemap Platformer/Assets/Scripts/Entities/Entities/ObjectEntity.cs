using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEntity : Entity
{
    // Update is called once per frame
    void Update()
    {
        _controller.move();
    }
}
