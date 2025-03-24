using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //public Slider healthBar;
    private Slider slider;
    public TextMeshProUGUI healthCounter;

    public GameObject playerStats;

    //Private player's health
    private float currentHealth, maxHealth;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = PlayerStats.Instance.currentHealth;

        maxHealth = PlayerStats.Instance.maxHealth;

        // Avoid division by zero and clamp value between 0 and 1
        float fillValue = Mathf.Clamp01(currentHealth / maxHealth);
        slider.value = fillValue;

        healthCounter.text = currentHealth.ToString("0") + " / " + maxHealth.ToString("0");
    }
}
