using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PotionData")]
public class PotionItem : ItemData {
    public int hpRecovery;
    public float damageBoostDuration;
    public int damageBoostAmount;
    public float speedBoostDuration;
    public int speedBoostAmount;
    private PlayerStats _playerStats;
    private InventoryManager _inventory;

    public override void UseItem()
    {
        _playerStats = PlayerStats.Instance;
        _inventory = InventoryManager.Instance;
        
        // Check if player and inventory are not null
        if (_playerStats == null)
        {
            Debug.LogError("PlayerStats is null!");
            return;
        }
        if (_inventory == null)
        {
            Debug.LogError("InventoryManager is null!");
            return;
        }

        // Increase health
        _playerStats.GainHealth(hpRecovery);

        // Apply temporary boosts
        _playerStats.ApplyTemporaryDamageBoost(damageBoostAmount, damageBoostDuration);
        _playerStats.ApplyTemporarySpeedBoost(speedBoostAmount, speedBoostDuration);

        // Remove this item from inventory
        _inventory.RemoveItem(this);

        
         //Destroy(this);
    }

}
