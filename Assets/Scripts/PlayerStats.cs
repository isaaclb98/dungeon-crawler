using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Singleton
    public static PlayerStats Instance { get; private set; }

    public PlayerData playerData;

    // Dynamic data
    public int currentLevel;
    public int currentXp;
    public int currentHealth;
    public int currentAttack;
    public int currentDefense;
    public int currentGold;
    public int xpToLevelUp;
    public int currentMaxHealth;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
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
        currentHealth += amount;
        Debug.Log("Gained health: " + (currentMaxHealth - amount));

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
}
