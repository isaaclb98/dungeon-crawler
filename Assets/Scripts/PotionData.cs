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

    public override void UseItem(PlayerStats player, InventoryManager inventory)
    {
        // Check if player and inventory are not null
        if (player == null)
        {
            Debug.LogError("PlayerStats is null!");
            return;
        }
        if (inventory == null)
        {
            Debug.LogError("InventoryManager is null!");
            return;
        }

        // Increase health
        player.GainHealth(hpRecovery);

        // Apply temporary boosts
        player.ApplyTemporaryDamageBoost(damageBoostAmount, damageBoostDuration);
        player.ApplyTemporarySpeedBoost(speedBoostAmount, speedBoostDuration);

        // Remove this item from inventory
        inventory.RemoveItem(this);

        // Optional: Optionally destroy the potion item object if no longer needed
        // Destroy(this);
    }

}
