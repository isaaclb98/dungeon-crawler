using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // List of items
    public List<StaticData.ItemData> inventoryItems = new List<StaticData.ItemData>();

    // Add an item to the inventory
    public void AddItem(StaticData.ItemData newItem)
    {
        inventoryItems.Add(newItem);
        Debug.Log("Added item: " + newItem.itemName);
    }

    // Remove an item from the inventory
    public void RemoveItem(StaticData.ItemData item)
    {
        if (inventoryItems.Contains(item))
        {
            inventoryItems.Remove(item);
            Debug.Log("Removed item: " + item.itemName);
        }
    }
}
