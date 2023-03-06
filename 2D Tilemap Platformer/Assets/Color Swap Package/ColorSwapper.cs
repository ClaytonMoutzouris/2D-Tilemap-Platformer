using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorSwapper
{

    public static void SwapPlayerColors(Material material, List<Color> colors)
    {

        foreach(PlayerColorSwapID swapID in System.Enum.GetValues(typeof(PlayerColorSwapID))) {
            material.SetColor(swapID.ToString(), colors[(int)swapID]);
        }

    }


}
