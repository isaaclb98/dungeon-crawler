using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Singleton
    public static PlayerStats Instance { get; private set; }

    public PlayerData playerData;

    // Dynamic data
    [HideInInspector]
    public int currentLevel;
    [HideInInspector]
    public int currentXp;
    [HideInInspector]
    public int currentHealth;
    [HideInInspector]
    public int currentAttack;
    [HideInInspector]
    public int currentDefense;
    [HideInInspector]
    public int currentGold;
    [HideInInspector]
    public int xpToLevelUp;
    [HideInInspector]
    public int currentMaxHealth;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("PlayerStats instance set: " + gameObject.name);
        }
        else
        {
            Debug.Log("Duplicate PlayerStats detected on: " + gameObject.name);
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Defaults
        currentLevel = playerData.startingLevel;
        currentXp = playerData.startingXp;
        currentHealth = playerData.startingHealth;
        currentAttack = playerData.startingAttack;
        currentDefense = playerData.startingDefense;
        xpToLevelUp = playerData.startingXpToLevelUp;
        currentGold = playerData.startingGold;
        currentMaxHealth = playerData.startingHealth;
        
        Debug.Log($"Player Stats - Level: {currentLevel}, XP: {currentXp}, Health: {currentHealth}, Attack: {currentAttack}, Defense: {currentDefense}, XP To Level Up: {xpToLevelUp}, Gold: {currentGold}, Max Health: {currentMaxHealth}");
    }

    // Player gains xp
    public void GainXp(int amount)
    {
        currentXp += amount;
        if (currentXp >= xpToLevelUp)
        {
            LevelUp();
        }
    }

    // Player gains gold
    public void GainGold(int amount)
    {
        currentGold += amount;
        Debug.Log("Gold Gained: " + amount + ", Total Gold: " + currentGold);
    }

    private void LevelUp()
    {
        currentLevel++;
        currentXp -= xpToLevelUp;

        // Eventually allow the player to choose what to increase
        currentMaxHealth += 1;
        currentAttack += 1;
        currentDefense += 1;

        // Recalculate XP needed for the next level using the multiplier from static data
        xpToLevelUp = Mathf.RoundToInt(xpToLevelUp * (float)playerData.levelUpXpNeededMultiplier);

        Debug.Log("Level Up! You are now level: " + currentLevel);
        Debug.Log("xp needed to level up: " + xpToLevelUp);
    }

    public void GainHealth(int amount)
    {
        Debug.Log($"GainHealth called on instance: {gameObject.name} with currentMaxHealth: {currentMaxHealth}");
        
        Debug.Log("current max health " + currentMaxHealth);
        Debug.Log("amount potion " + amount);
        Debug.Log("Gained health: " + (currentMaxHealth - amount));

        currentHealth += amount;

        if (currentHealth > currentMaxHealth)
        {
            currentHealth = currentMaxHealth;
        }
    }

    public void ApplyTemporaryDamageBoost(int amount, float duration)
    {
        // to do
    }
    
    public void ApplyTemporarySpeedBoost(int amount, float duration)
    {
        // to do
    }

    private float damageCooldown = 1.5f; // 1 second cooldown
    private float lastDamageTime = 0f;

    public void TakeDamage(int damage)
    {
        if (Time.time < lastDamageTime + damageCooldown) return; // Prevent taking damage too fast

        lastDamageTime = Time.time; // Update last damage time
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, currentMaxHealth); // Prevent negative health

        Debug.Log($"Player took {damage} damage. Current Health: {currentHealth}");

        if (currentHealth > 0)
        {
            Debug.Log("Player is still alive.");
        }
        else
        {
            Debug.Log("Player's health reached 0. Calling Die().");
            Die();
        }
    }



    private void Die()
    {
        Debug.Log("Player has died!");

        // Reset inventory
        if (InventoryManager.Instance)
        {
            InventoryManager.Instance.ResetInventory();
        }

        ResetPlayerStats();

        // Destroy old player instance before restarting
        Destroy(gameObject);

        // Restart the game
        GameManager.Instance.RestartGame();
    }


    private void ResetPlayerStats()
    {
        currentHealth = playerData.startingHealth;
        currentGold = playerData.startingGold;
        currentXp = playerData.startingXp;
        currentLevel = playerData.startingLevel;
        currentAttack = playerData.startingAttack;
        currentDefense = playerData.startingDefense;
        currentMaxHealth = playerData.startingHealth;
    }



}
