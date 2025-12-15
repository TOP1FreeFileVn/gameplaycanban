using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ZoneData", menuName = "Game/ZoneData")]
public class ZoneData : ScriptableObject
{
    public string zoneName;
    public Sprite background;
    public AudioClip bgMusic;
    public LevelData[] levels;
}
