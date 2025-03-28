using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/KeyItem")]
public class KeyItem : ItemData
{
    private InventoryManager _inventory;

    public override void UseItem()
    {
        Debug.Log("This can be used to unlock a door.");
    }

    public void RemoveItem()
    {
        _inventory = InventoryManager.Instance;
        
        // Remove this item from inventory
        _inventory.RemoveItem(this);
    }
}