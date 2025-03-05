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

    public float attackSpeed = 1.0f; // public float attackSpeed = 1.0f;

}
