using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    
    // List of items
    public List<ItemData> inventoryItems = new List<ItemData>();

    public Transform ItemContent;
    public GameObject Items;
    private ItemData selectedItem;  // Store the currently selected item
    // Weapon-related
    public WeaponData defaultWeapon;
    public WeaponData equippedWeapon;
    public Transform weaponHolder;
    public GameObject currentWeaponPrefab; 

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        EquipWeapon(defaultWeapon);
    }
    
    // Use an item (this calls the item's own UseItem method).
    public void SetSelectedItem(ItemData item)
    {
        selectedItem = item;
    }

    public void UseSelectedItem()
    {
        if (selectedItem != null)
        {
            UseItem(selectedItem);
        }
    }
    public void UseItem(ItemData item)
    {
        item.UseItem();
        
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
        // Clear previous items in the inventory UI
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        // Create UI buttons for each inventory item
        foreach (var item in inventoryItems)
        {
            // Check for null item
            if (item == null)
            {
                Debug.LogError("Inventory item is null.");
                continue; // Skip this item if it's null
            }

            GameObject obj = Instantiate(Items, ItemContent);

            // Set item name and icon
            var itemName = obj.transform.Find("ItemName")?.GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon")?.GetComponent<Image>();

            // Check if the components exist before setting values
            if (itemName != null && itemIcon != null)
            {
                itemName.text = item.itemName;
                itemIcon.sprite = item.icon;
            }
            else
            {
                Debug.LogError("Missing ItemName or ItemIcon in the prefab.");
            }

            // Add a click event to the button
            Button itemButton = obj.GetComponent<Button>();
            if (itemButton != null)
            {
                itemButton.onClick.AddListener(() => UseItem(item));
                itemButton.onClick.AddListener(() => Destroy(obj));
            }
            else
            {
                Debug.LogError("Button component not found on item object.");
            }
        }
    }


    // Return the currently equipped weapon.
    public WeaponData GetEquippedWeapon() {
        return equippedWeapon;
    }

    // Equip a new weapon.
    public void EquipWeapon(WeaponData weapon) {
        // If a weapon is already equipped, add it back into the inventory.
        if (equippedWeapon != null) {
            AddItem(equippedWeapon);
        }

        // Destroy any existing weapon instance.
        if (currentWeaponPrefab != null) {
            Destroy(currentWeaponPrefab);
        }

        // Instantiate the new weapon prefab as a child of weaponHolder.
        currentWeaponPrefab = Instantiate(weapon.prefab, weaponHolder);
        // Set proper local transform values (adjust as needed).
        currentWeaponPrefab.transform.localPosition = new Vector3(0.2f, -0.7f, 0.6f);
        currentWeaponPrefab.transform.localRotation = Quaternion.Euler(0, 75, 0);
        currentWeaponPrefab.transform.localScale = new Vector3(1.2f, 1.6f, 1.5f);

        equippedWeapon = weapon;
        Debug.Log("Equipped new weapon: " + equippedWeapon.itemName);
    }

    public void ResetInventory()
    {
    Debug.Log("Resetting inventory...");

    if (inventoryItems == null)
    {
        Debug.LogWarning("Inventory items list is null, initializing it.");
        inventoryItems = new List<ItemData>(); // Ensure it's initialized
    }
    else
    {
        inventoryItems.Clear();
    }

    // Reset equipped weapon to default
    if (defaultWeapon != null)
    {
        EquipWeapon(defaultWeapon);
    }
    else
    {
        Debug.LogWarning("Default weapon is null! Cannot equip.");
    }
    }


}
