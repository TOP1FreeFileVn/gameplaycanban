using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WaveData", menuName = "Game/WaveData")]
public class WaveData : ScriptableObject
{
    [Header("Enemy Spawm")]
    public GameObject enemyPrefab;
    public int enemyCount = 10;
    public float spawnInterval = 1.5f;
    [Header("Enemy Scaling")]
    public float hpMultiple = 1f;
    public float speedMultiplier = 1f;
}
