using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackoutBar : MonoBehaviour
{
    public Image bar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBar(float percent)
    {
        bar.rectTransform.localScale = new Vector3(percent, 1);
    }
}
