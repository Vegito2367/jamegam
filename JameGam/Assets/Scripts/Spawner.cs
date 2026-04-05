using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject easyEnemyPrefab;
    public GameObject mediumEnemyPrefab;
    public GameObject hardEnemyPrefab;

    [Header("Spawn Settings")]
    public Transform[] spawnPoints;
    public float timeBetweenSpawns = 0.25f;
    public float timeBetweenWaves = 1.5f;
    public bool waitForWaveToBeCleared = true;

    [Header("Wave UI")]
    public GameObject wave1UI;
    public GameObject wave2UI;
    public GameObject wave3UI;
    public float waveUIDisplayTime = 1f;

    [Header("Wave 1 Counts")]
    public int wave1EasyCount = 10;
    public int wave1MediumCount = 0;
    public int wave1HardCount = 0;

    [Header("Wave 2 Counts")]
    public int wave2EasyCount = 5;
    public int wave2MediumCount = 2;
    public int wave2HardCount = 0;

    [Header("Wave 3 Counts")]
    public int wave3EasyCount = 2;
    public int wave3MediumCount = 0;
    public int wave3HardCount = 3;

    readonly List<GameObject> aliveEnemies = new List<GameObject>();

    void Start()
    {
        StartCoroutine(RunWaves());
    }

    IEnumerator RunWaves()
    {
        yield return StartCoroutine(ShowWaveUI(wave1UI));
        yield return StartCoroutine(SpawnWave(wave1EasyCount, wave1MediumCount, wave1HardCount));
        yield return StartCoroutine(HandleWaveTransition());

        yield return StartCoroutine(ShowWaveUI(wave2UI));
        yield return StartCoroutine(SpawnWave(wave2EasyCount, wave2MediumCount, wave2HardCount));
        yield return StartCoroutine(HandleWaveTransition());

        yield return StartCoroutine(ShowWaveUI(wave3UI));
        yield return StartCoroutine(SpawnWave(wave3EasyCount, wave3MediumCount, wave3HardCount));
        yield return StartCoroutine(HandleWaveTransition());
    }

    IEnumerator ShowWaveUI(GameObject waveUI)
    {
        SetAllWaveUIInactive();

        if (waveUI == null)
        {
            yield break;
        }

        waveUI.SetActive(true);
        yield return new WaitForSeconds(waveUIDisplayTime);
        waveUI.SetActive(false);
    }

    IEnumerator SpawnWave(int easyCount, int mediumCount, int hardCount)
    {
        for (int i = 0; i < easyCount; i++)
        {
            SpawnEnemy(easyEnemyPrefab);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        for (int i = 0; i < mediumCount; i++)
        {
            SpawnEnemy(mediumEnemyPrefab);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        for (int i = 0; i < hardCount; i++)
        {
            SpawnEnemy(hardEnemyPrefab);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    IEnumerator HandleWaveTransition()
    {
        if (waitForWaveToBeCleared)
        {
            yield return new WaitUntil(AllSpawnedEnemiesAreDefeated);
        }

        yield return new WaitForSeconds(timeBetweenWaves);
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("Spawner: Missing enemy prefab reference.", this);
            return;
        }

        Vector3 spawnPosition = transform.position;
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            Transform chosenPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            if (chosenPoint != null)
            {
                spawnPosition = chosenPoint.position;
            }
        }

        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        aliveEnemies.Add(spawnedEnemy);
    }

    bool AllSpawnedEnemiesAreDefeated()
    {
        aliveEnemies.RemoveAll(enemy => enemy == null);
        return aliveEnemies.Count == 0;
    }

    void SetAllWaveUIInactive()
    {
        if (wave1UI != null)
        {
            wave1UI.SetActive(false);
        }

        if (wave2UI != null)
        {
            wave2UI.SetActive(false);
        }

        if (wave3UI != null)
        {
            wave3UI.SetActive(false);
        }
    }
}
