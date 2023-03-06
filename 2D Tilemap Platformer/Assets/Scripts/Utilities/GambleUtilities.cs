using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GambleUtilities
{

    public static float GetGravityModifier(PhysicsBody2D body2D)
    {
        return body2D.gravityMod * GambleConstants.GRAVITY;
    }

}
