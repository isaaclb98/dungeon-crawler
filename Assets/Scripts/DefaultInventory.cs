using UnityEngine;

[CreateAssetMenu(fileName = "InventoryConfig", menuName = "Config/InventoryConfig")]
public class InventoryConfig : ScriptableObject
{
    public WeaponData defaultWeapon;
}