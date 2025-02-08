using UnityEngine;
using UnityEngine.UI;  // For accessing UI elements

public class HealthStaminaBarController : MonoBehaviour
{
    public Image healthFill;   // The Image for health fill
    public Image staminaFill;  // The Image for stamina fill

    public float maxHealth = 100f;
    public float maxStamina = 100f;

    private float currentHealth;
    private float currentStamina;

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;

        // Initialize the fill amounts
        UpdateHealthBar();
        UpdateStaminaBar();
    }

    void Update()
    {
        // Example: Reduce health and stamina over time (for testing purposes)
        if (currentHealth > 0) currentHealth -= Time.deltaTime * 5f;  // Decrease health
        if (currentStamina > 0) currentStamina -= Time.deltaTime * 2f;  // Decrease stamina

        // Update the UI images based on current health and stamina
        UpdateHealthBar();
        UpdateStaminaBar();
    }

    // Update the health fill based on current health
    public void UpdateHealth(float newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, 0f, maxHealth);
        UpdateHealthBar();
    }

    // Update the stamina fill based on current stamina
    public void UpdateStamina(float newStamina)
    {
        currentStamina = Mathf.Clamp(newStamina, 0f, maxStamina);
        UpdateStaminaBar();
    }

    // Set the fill amount for the health bar
    private void UpdateHealthBar()
    {
        healthFill.fillAmount = currentHealth / maxHealth;  // Fill amount ranges from 0 to 1
    }

    // Set the fill amount for the stamina bar
    private void UpdateStaminaBar()
    {
        staminaFill.fillAmount = currentStamina / maxStamina;  // Fill amount ranges from 0 to 1
    }
}
