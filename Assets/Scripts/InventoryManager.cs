using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // List of items
    public List<ItemData> inventoryItems = new List<ItemData>();

    // Add an item to the inventory
    public void AddItem(ItemData newItem)
    {
        inventoryItems.Add(newItem);
        Debug.Log("Added item: " + newItem.itemName);
    }

    // Remove an item from the inventory
    public void RemoveItem(ItemData item)
    {
        if (inventoryItems.Contains(item))
        {
            inventoryItems.Remove(item);
            Debug.Log("Removed item: " + item.itemName);
        }
    }
}
