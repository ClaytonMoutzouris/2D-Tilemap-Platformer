using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPortrait : MonoBehaviour
{
    public ColorSwap colorSwap;
    public Image image;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void LoadPortrait()
    {
        image.material = new Material(image.material);
        colorSwap = new ColorSwap(image.material);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
