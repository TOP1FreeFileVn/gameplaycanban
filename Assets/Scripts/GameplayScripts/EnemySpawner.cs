using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Data Reference")]
    public LevelData currentLevelData;
    [Header("Spawn")]
    [SerializeField] private float _spawnDistanceX = 7f;
    [SerializeField] private float _vertiacalExtentY = 8f;
    private const int NUMBER_OF_SPAWN = 6;
    private float _segmentHeight;
    public Transform player;
    [Header("Runtime State")]
    [SerializeField] private int currentStepIndex = 0;
    [SerializeField] private int currentWaveIndex = 0;
    [SerializeField] private int enemySpawnedInWave = 0;
    [SerializeField] private float spawnTimer = 0f;
    [SerializeField] private bool isStepsFinished = false;
    [SerializeField] private float distanceGone;
    private void Start()
    {
        _segmentHeight = _vertiacalExtentY / NUMBER_OF_SPAWN;
        currentStepIndex = 0;
        currentWaveIndex = 0;
        enemySpawnedInWave = 0;
        spawnTimer = 0f;
        isStepsFinished = false;

        if (currentLevelData == null)
        {
            Debug.Log("Thieu level nha tinh yeu");
        }
    }
    void Update()
    {
        if (isStepsFinished) return;
        if (currentLevelData == null) return;
        StepData currentStep = currentLevelData.steps[currentStepIndex];
        if (distanceGone > currentStep.length * (currentStepIndex + 1) ||  currentStep.waves == null || currentStep.waves.Length == 0)
        {
            NextStep();
            return;
        }
        WaveData currentWave = currentStep.waves[currentWaveIndex];
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= currentWave.spawnInterval)
        {
            SpawnEnemy(currentWave);
            spawnTimer = 0f;
        }
    }
    public void GetDistanceGone(float dis) {
        distanceGone = dis;
    }
    private Vector3 GetSpawnPosition()
    {
        if(player == null)
        {
            return transform.position;
        }
        int segmentIndex = Random.Range(0, NUMBER_OF_SPAWN);
        float spawnCenter = player.position.y;
        float segmentBaseY = spawnCenter - (_vertiacalExtentY / 2f);
        float segmentStartY = segmentBaseY + (segmentIndex * _segmentHeight);
        float randomY = Random.Range(segmentStartY,segmentStartY + _segmentHeight);
        float spawnX = player.position.x - _spawnDistanceX;
        return new Vector3(spawnX,randomY,player.position.z);

    }
    private void SpawnEnemy(WaveData waveInfo)
    {
        if(enemySpawnedInWave >= waveInfo.enemyCount)
        {
            NextWave();
            return;
        }
        Vector3 spawnPosition = GetSpawnPosition();
        if(waveInfo.enemyPrefab != null)
        {
            GameObject newEnemy = Instantiate(waveInfo.enemyPrefab,spawnPosition,Quaternion.identity);
            EnemyStats stats = newEnemy.GetComponent<EnemyController>().enemyStats;
            Debug.Log($"Da trieu hoi ra quai thu : {newEnemy.name} | Step: {currentStepIndex + 1} | Wave: {currentWaveIndex + 1}");
        }
        enemySpawnedInWave++;
    }
    private void NextWave()
    {
        Debug.Log("Het wave");
        currentWaveIndex++;
        enemySpawnedInWave = 0;
        spawnTimer = 0f;
        StepData currentStep = currentLevelData.steps[currentStepIndex];
    }
    private void NextStep()
    {
        Debug.Log("Het step nha");
        currentStepIndex++;
        currentWaveIndex = 0;
        enemySpawnedInWave = 0;
        if(currentStepIndex >= currentLevelData.steps.Length)
        {
            FinishSteps();
        }
    }

    private void FinishSteps()
    {
        isStepsFinished = true;
    }

}
