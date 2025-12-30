using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyStats", menuName = "Game/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public float baseHealth;
    public float moveSpeed;
    public float attackRange;
    public float attackSpeed;
}
