using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelProgressUI : MonoBehaviour
{
    [Header("References")]
    public RectTransform progressBarRect;
    public RectTransform playerIcon;
    public RectTransform flagsContainer;
    public GameObject flagPrefab;
    public Image fillBar;
    [Header("Data")]
    public float totalLevelLength = 0f;
    private float currentDistance = 0f;
    
    public void SetUpUI(LevelData levelData)
    {
        foreach(Transform child in flagsContainer)
        {
            Destroy(child.gameObject);
        }
        totalLevelLength = 0f;
        foreach (var step in levelData.steps)
        {
            totalLevelLength += step.length;
        }
        float currentAccumlatedLength = 0f;
        float barWidth = progressBarRect.rect.width;

        for(int i = 0; i < levelData.steps.Length; i++)
        {
            float stepLength = levelData.steps[i].length;
            currentAccumlatedLength+= stepLength;
            float ratio = currentAccumlatedLength / totalLevelLength;
            GameObject newFlag = Instantiate(flagPrefab, flagsContainer);
            RectTransform flagRect = newFlag.GetComponent<RectTransform>();
            float xOffSet = totalLevelLength / 5;
            float xPosx = barWidth * ratio;
            flagRect.anchoredPosition = new Vector2(xPosx - xOffSet, 0f);

        }
        UpdateProgress(0);
    }
    public void UpdateProgress(float distanceTraveled)
    {
        currentDistance = distanceTraveled;
        float ratio = Mathf.Clamp01(currentDistance / totalLevelLength);
        float barWidth = progressBarRect.rect.width;
        playerIcon.anchoredPosition = new Vector2(barWidth * ratio, 0f);
        if(fillBar != null)
        {
            fillBar.fillAmount = ratio;
        }
    }
}
