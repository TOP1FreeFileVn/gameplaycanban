using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerStatsData", menuName = "Game/PlayerStatsData")]
public class PlayerStatsData : ScriptableObject
{
 
    public float maxHealth = 100f;
    public float moveSpeed = 5f;
}
