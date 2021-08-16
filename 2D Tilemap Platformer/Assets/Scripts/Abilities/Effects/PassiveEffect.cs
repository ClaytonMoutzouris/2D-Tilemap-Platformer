using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveEffect : Effect
{

    public override void Apply(Entity effected, Entity effector = null)
    {
        base.Apply(effected, effector);

    }

    public override void Remove()
    {
        //Remove from players list of effects, then garbage collect?
        //Destroy(this);

    }
}
