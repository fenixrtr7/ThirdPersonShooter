using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    Slider healthBar;
    public static int currentHealth;

    // Start is called before the first frame update
    void Awake()
    {
        healthBar = GetComponent<Slider>();
        currentHealth = 100;
    }

    public void ReduceHealth()
    {
        currentHealth-= 50;
        healthBar.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = currentHealth;
    }
}
