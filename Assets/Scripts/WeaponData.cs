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
}
