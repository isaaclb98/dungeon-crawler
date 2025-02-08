using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerData playerData;

    // Dynamic data
    public int currentLevel;
    public int currentXp;
    public int currentHealth;
    public int currentAttack;
    public int currentDefense;
    public int xpToLevelUp;

    void Start()
    {
        // Initialize default stats
        // Need to implement saving feature eventually
        currentLevel = playerData.startingLevel;
        currentXp = playerData.startingXp;
        currentHealth = playerData.startingHealth;
        currentAttack = playerData.startingAttack;
        currentDefense = playerData.startingDefense;
        xpToLevelUp = playerData.startingXpToLevelUp;
    }

    // Call this when the player earns XP (e.g., after defeating an enemy)
    public void GainXp(int amount)
    {
        currentXp += amount;
        if (currentXp >= xpToLevelUp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        currentXp -= xpToLevelUp;

        // Eventually allow the player to choose what to increase
        currentHealth += 1;
        currentAttack += 1;
        currentDefense += 1;

        // Recalculate XP needed for the next level using the multiplier from static data
        xpToLevelUp = Mathf.RoundToInt(xpToLevelUp * (float)playerData.levelUpXpNeededMultiplier);

        Debug.Log("Level Up! You are now level: " + currentLevel);
    }
}
