using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Data/WeaponData")]
public class WeaponData : ItemData
{
    public float range;
    public int damage;
    public string weaponType;
    public GameObject prefab;
    public Vector3 weaponScale = new Vector3(1f, 1f, 1f); //Custom weapon scale
    public float attackSpeed = 1.0f;

    public override void UseItem(PlayerStats player, InventoryManager inventory)
    {
        // If a weapon is already equipped, add it back to the inventory.
        if (inventory.GetEquippedWeapon() != null)
        {
            inventory.AddItem(inventory.GetEquippedWeapon());
        }

        // Equip the new weapon.
        inventory.EquipWeapon(this);

        // Remove this weapon item from the inventory
        inventory.RemoveItem(this);
    }
}
