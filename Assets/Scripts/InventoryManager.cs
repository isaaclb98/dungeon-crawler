using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    // List of items
    public List<ItemData> inventoryItems = new List<ItemData>();

    public Transform ItemContent;
    public GameObject Items;

    private void Awake()
    {
        Instance = this;
    }
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
    
    public override string ToString()
    {
        if (inventoryItems == null || inventoryItems.Count == 0)
            return "Inventory is empty.";

        StringBuilder sb = new StringBuilder("Inventory:\n");
        foreach (ItemData item in inventoryItems)
        {
            sb.AppendLine(item.itemName);
        }
        return sb.ToString();
    }
    public void ListItems()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in inventoryItems)
        {
            GameObject obj = Instantiate(Items, ItemContent);
            var itemName = obj.transform.Find("IteName").GetComponent<Text>();
            var iteIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            itemName.text = item.itemName;
            iteIcon.sprite = item.icon;
        }
    }
}
