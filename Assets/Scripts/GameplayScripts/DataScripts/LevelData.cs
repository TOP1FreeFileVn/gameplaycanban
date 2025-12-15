using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelData",menuName = "Game/LevelData")]
public class LevelData : ScriptableObject
{
    public int levelIndex;
    public string description;
    [Header("Structure")]
    public StepData[] steps;
}
    

