using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelProgressUI progressUI;
    public LevelData currentLevel;
    public Transform playerTransform;
    private bool isLevelFinished;
    private float maxLevelLength = 0f;
    public float distanceGone = 0f;
    private float startZ;
    public PlayerController player;
    public EnemySpawner enemySpawner;
    void Start()
    {
        isLevelFinished = false;
        startZ = playerTransform.position.z;
        progressUI.SetUpUI(currentLevel);
        maxLevelLength = progressUI.totalLevelLength;

    }

    // Update is called once per frame
    void Update()
    {
        if (isLevelFinished) return; 
        distanceGone = distanceGone+ player.currentSpeed * Time.deltaTime;
        progressUI.UpdateProgress(distanceGone);
        enemySpawner.GetDistanceGone(distanceGone);
        if (distanceGone >= maxLevelLength)
        {
            WinGame();
        }
    }
    private void WinGame()
    {
        isLevelFinished = true;
        Debug.Log("Win");
    }
}
