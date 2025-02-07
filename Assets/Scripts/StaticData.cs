using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticData", menuName = "Data/StaticData")]
public class StaticData : ScriptableObject
{
    // Default player stats
    [System.Serializable]
    public class StaticPlayerStats
    {
        public int startingLevel = 1;
        public int startingHealth = 10;
        public int startingAttack = 1;
        public int startingDefense = 1;
        public int startingXp = 0;
        public int startingXpToLevelUp = 83;
    
        // Player levelling constants
        public double levelUpXpNeededMultiplier = 1.1;
    }
    
    public StaticPlayerStats playerStats;
    
    // Enemy stats
    [System.Serializable]
    public class EnemyStats
    {
        public string name;
        public int health;
        public int attack;
        public int xpReward;
        public int goldReward;
    }

    public EnemyStats[] enemies;

    // Weapon stats
    
    // Items
    [System.Serializable]
    public class ItemData
    {
        public string itemName;
        public int itemID;
        public Sprite icon;
        public string description;
    }
    public ItemData[] items;
}
