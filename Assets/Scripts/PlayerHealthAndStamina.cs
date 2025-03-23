using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthAndStamina : MonoBehaviour
{
    //public Slider healthBar;
    private Slider slider;
    public Text healthCounter;

    //public Slider staminaBar;
    public PlayerData playerData;
    public static PlayerHealthAndStamina Instance { get; set; } 

    public float maxHealth;
    public float maxStamina;

    private float currentHealth;
    private float currentStamina;

    public float staminaRegenRate;
    public float staminaDrainRate;

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        UpdateUI();
    }

    void Update()
    {
        /*if(currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
        UpdateUI();*/

        if(Input.GetKeyDown(KeyCode.G))
        {
            currentHealth -= 10;
        }
    }

    void UpdateUI()
    {
        /*healthBar.value = currentHealth / maxHealth;
        staminaBar.value = currentStamina / maxStamina;*/
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Player Current Health: " + currentHealth);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
    }

    public void UseStamina(float amount)
    {
        currentStamina -= amount * staminaDrainRate * Time.deltaTime;
        //currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        UpdateUI();
    }

    public float GetCurrentHealth()  // Public getter for currentHealth
    {
        return currentHealth;
    }
}
