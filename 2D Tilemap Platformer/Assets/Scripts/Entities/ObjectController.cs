using UnityEngine;
using System.Collections;


public class ObjectController : Entity
{

    protected override void Awake()
    {
        base.Awake();
    }

    // the Update loop contains a very simple example of moving the character around and controlling the animation
    void Update()
    {


        _controller.move();

    }

}
