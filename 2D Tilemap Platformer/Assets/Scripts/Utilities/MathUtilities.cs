using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtilities
{
    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static float Vector2Degree(Vector2 vector)
    {
        float value = (float)((Mathf.Atan2(vector.x, vector.y) / Math.PI) * 180f);
        if (value < 0) value += 360f;

        return value;
    }

}
