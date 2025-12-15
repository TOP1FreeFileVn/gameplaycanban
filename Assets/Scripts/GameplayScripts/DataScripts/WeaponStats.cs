using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WeaponStats",menuName = "Game/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    public float fireRate;
    public float bulletSpeed;
    public GameObject bulletPrefab;
}
