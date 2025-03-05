using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    
    // List of items
    public List<ItemData> inventoryItems = new List<ItemData>();

    public Transform ItemContent;
    public GameObject Items;
    
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
            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }
    }
    
    // Use an item (this calls the item's own UseItem method).
    public void UseItem(ItemData item, PlayerStats player) {
        item.UseItem(player, this);
    }

    // Return the currently equipped weapon.
    public WeaponData GetEquippedWeapon() {
        return equippedWeapon;
    }

    // Equip a new weapon.
    public void EquipWeapon(WeaponData newWeapon) {
        // If a weapon is already equipped, add it back into the inventory.
        if (equippedWeapon != null) {
            AddItem(equippedWeapon);
        }

        // Destroy any existing weapon instance.
        if (currentWeaponPrefab != null) {
            Destroy(currentWeaponPrefab);
        }

        // Instantiate the new weapon prefab as a child of weaponHolder.
        currentWeaponPrefab = Instantiate(newWeapon.prefab, weaponHolder);
        // Set proper local transform values (adjust as needed).
        currentWeaponPrefab.transform.localPosition = new Vector3(0.2f, -0.7f, 0.6f);
        currentWeaponPrefab.transform.localRotation = Quaternion.Euler(0, 75, 0);
        currentWeaponPrefab.transform.localScale = new Vector3(1.2f, 1.6f, 1.5f);

        equippedWeapon = newWeapon;
        Debug.Log("Equipped new weapon: " + equippedWeapon.itemName);
    }
}
