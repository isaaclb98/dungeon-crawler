using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthAndStamina : MonoBehaviour
{
    public Slider healthBar;
    public Slider staminaBar;

    public float maxHealth = 100f;
    public float maxStamina = 100f;

    private float currentHealth;
    private float currentStamina;

    public float staminaRegenRate = 5f;
    public float staminaDrainRate = 20f;

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        UpdateUI();
    }

    void update()
    {
        if(currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        healthBar.value = currentHealth / maxHealth;
        staminaBar.value = currentStamina / maxStamina;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
    }

    public void UseStamina(float amount)
    {
        currentStamina -= amount * staminaDrainRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        UpdateUI();
    }
}
