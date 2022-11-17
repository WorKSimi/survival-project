using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponSystemData/WeaponData")]

public class WeaponData : InventoryItemData
{
    public string weaponName;
    public string weaponType;
    public double weaponDamage;
}
