using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "StepData" , menuName = "Game/StepData")]
public class StepData : ScriptableObject
{
    public int stepIndex;
    public float length;
    [Header("Wave Data")]
    public WaveData[] waves;
}

