using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDatabase",menuName="Game/Level Database")]
public class LevelDatabase : ScriptableObject
{
    [Header("World Map Config")]
    public ZoneData[] zones;
    public ZoneData GetZone(int index)
    {
        if (index < 0 || index >= zones.Length)
        {
            Debug.LogWarning($"Không tìm thấy zone");
            return zones.Length > 0 ? zones[0] : null;
        }
        return zones[index];
    }
    public int GetTotalLevelsCount()
    {
        int total = 0;
        foreach(var zone in zones)
        {
            if(zone != null && zone.levels != null)
            {
                total += zone.levels.Length;
            }
        }
        return total;
    }
}
