using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image bar;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        if(maxHealth == 0)
        {
            bar.transform.localScale = new Vector3(0, 1);
        }
        else
        {
            bar.transform.localScale = new Vector3((currentHealth / maxHealth), 1);
        }
        text.text = currentHealth + " / " + maxHealth;
    }
}
