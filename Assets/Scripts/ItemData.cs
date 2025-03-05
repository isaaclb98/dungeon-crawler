using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Data/ItemData")]
public abstract class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public string description;
    
    public abstract void UseItem(PlayerStats player, InventoryManager inventory);
}
