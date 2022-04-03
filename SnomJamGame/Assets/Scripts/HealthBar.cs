using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider bar;
    
    void Start()
    {
        bar.maxValue = 1;
    }

    public void OnHealthChange(float curHealth, float maxHealth)
    {
        if (maxHealth <= 0)
            return;
        
        bar.value = curHealth / maxHealth;
    }
}
