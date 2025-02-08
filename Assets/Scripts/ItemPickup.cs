using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Reference to the potion's static data (assign this in the Inspector)
    public ItemData itemData;
    
    public AudioClip pickupSound;

    public void Pickup()
    {
        // Play the pickup sound at the object's position
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }
        
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager != null)
        {
            inventoryManager.AddItem(itemData);
            Debug.Log("Picked up " + itemData.itemName);
        }
        else
        {
            Debug.LogWarning("InventoryManager not found");
        }

        Destroy(gameObject);
    }
}
