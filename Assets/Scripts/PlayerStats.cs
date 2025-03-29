using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Singleton
    public static PlayerStats Instance { get; private set; }

    public PlayerData playerData;
    public PlayerUIManager uiManager;
    
    // Dynamic data
    [HideInInspector]
    public float currentLevel;
    [HideInInspector]
    public float currentXp;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentAttack;
    [HideInInspector]
    public float currentDefense;
    [HideInInspector]
    public float currentGold;
    [HideInInspector]
    public float xpToLevelUp;
    [HideInInspector]
    public float currentMaxHealth;

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

        //Health
        currentAttack = playerData.startingAttack;
        currentDefense = playerData.startingDefense;
        xpToLevelUp = playerData.startingXpToLevelUp;
        currentGold = playerData.startingGold;
        currentMaxHealth = playerData.startingHealth;
        currentHealth = currentMaxHealth;
        
        Debug.Log($"Player Stats - Level: {currentLevel}, XP: {currentXp}, Health: {currentHealth}, Attack: {currentAttack}, Defense: {currentDefense}, XP To Level Up: {xpToLevelUp}, Gold: {currentGold}, Max Health: {currentMaxHealth}");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GainXp(100.0f);
        }
    }

    // Player gains xp
    public void GainXp(float amount)
    {
        uiManager.ShowPopupText("+" + amount + " XP", Color.green);
        
        currentXp += amount;
        if (currentXp >= xpToLevelUp)
        {
            LevelUp();
        }
    }

    // Player gains gold
    public void GainGold(float amount)
    {
        uiManager.ShowPopupText("+" + amount + " Gold", Color.yellow);
        
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
        
        uiManager.ShowPopupText("Level up! You are now level " + currentLevel, Color.magenta);

        // Recalculate XP needed for the next level using the multiplier from static data
        xpToLevelUp = Mathf.RoundToInt(xpToLevelUp * (float)playerData.levelUpXpNeededMultiplier);

        Debug.Log("Level Up! You are now level: " + currentLevel);
        Debug.Log("xp needed to level up: " + xpToLevelUp);
    }

    public void GainHealth(float amount)
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

    public void ApplyTemporaryDamageBoost(float amount, float duration)
    {
        // to do
    }
    
    public void ApplyTemporarySpeedBoost(float amount, float duration)
    {
        // to do
    }

    private float damageCooldown = 1.5f; // 1 second cooldown
    private float lastDamageTime = 0f;

    public void TakeDamage(float damage)
    {
        if (Time.time < lastDamageTime + damageCooldown) return; // Prevent taking damage too fast

        lastDamageTime = Time.time; // Update last damage time
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, currentMaxHealth); // Prevent negative health

        Debug.Log($"Player took {damage} damage. Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }

        if (SoundManager.Instance)
        {
            SoundManager.Instance.PlaySound3D("Hurting");
        }
    }
    
    public void Die()
    {
        // Reset inventory
        if (InventoryManager.Instance)
        {
            InventoryManager.Instance.ResetInventory();
        }

        GameManager.Instance.lastDeathLevel = currentLevel;
        GameManager.Instance.lastDeathGold = currentGold;

        // Restart the game
        GameManager.Instance.LoadDeathScreen();
        
        // Destroy old player instance before restarting
        ResetPlayerStats();
        Destroy(gameObject);
    }
    
    public void Win()
    {
        // Reset inventory
        if (InventoryManager.Instance)
        {
            InventoryManager.Instance.ResetInventory();
        }

        GameManager.Instance.lastDeathLevel = currentLevel;
        GameManager.Instance.lastDeathGold = currentGold;

        // Restart the game
        GameManager.Instance.LoadWinScreen();
        
        // Destroy old player instance before restarting
        ResetPlayerStats();
        Destroy(gameObject);
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
