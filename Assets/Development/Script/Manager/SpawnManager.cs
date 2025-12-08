using System.Collections;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    private static StageSettingSO stageSettings;
    private int[][] enemyRatio;
    private int totalEnemies;
    int[] spawnedEnemyRatio;
    private GameObject[] monsterPrefabs;
    private GameObject bossPrefab;
    public GameObject[] itemPrefabs;
    public GameObject spawnPortal;
    public float spawnRate = 0.2f;
    private int currentStage => GameProgressManager.Instance.CurrentStage;
    private int currentRound => GameProgressManager.Instance.CurrentRound;

    private int enemyCount = 0; // 현재 라운드에서 소환된 적 수
    private int roundTotalKillCount = 0; // 현재 라운드에서 처치한 적 수

    protected override void Awake()
    {
        base.Awake();
        // SetStage(1, 1);
        StartCoroutine(Spawn_Monster_Coroutine());
        // StartCoroutine(Spawn_Item_Coroutine());
    }

    void Update()
    {

    }
    IEnumerator Spawn_Monster_Coroutine()
    {

        if (GameManager.instance != null && GameManager.instance.CurrentState == GameState.GAME_PLAY && totalEnemies > enemyCount)
        {

            if (!spawnPortal.activeSelf)
                spawnPortal.SetActive(true);


            float xPos = Random.Range(-4.0f, 4.0f);
            // float zPos = Random.Range(33.5f, 55.5f);
            Vector3 spawnPosition = new Vector3(xPos, 0.32f, 50f);
            if (currentRound == 2 && enemyCount < 1)
            {
                Instantiate(bossPrefab, spawnPosition, Quaternion.Euler(0, 180, 0));

            }
            if (enemyCount % 10 > 3)
            {

                for (int i = 0; i < monsterPrefabs.Length; i++)
                {
                    if (enemyRatio[currentRound][i] > spawnedEnemyRatio[i] / enemyCount * 100)
                    {
                        Instantiate(monsterPrefabs[i], spawnPosition, Quaternion.Euler(0, 180, 0));
                        break;
                    }
                }

            }
            else
                Instantiate(monsterPrefabs[Random.Range(0, monsterPrefabs.Length)], spawnPosition, Quaternion.Euler(0, 180, 0));

            enemyCount++;
            float wait = Random.Range(4f, 5f) / spawnRate;
            yield return new WaitForSeconds(wait);
        }
        else
        {
            spawnPortal.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
        StartCoroutine(Spawn_Monster_Coroutine());
    }

    IEnumerator Spawn_Item_Coroutine()
    {
        if (GameManager.instance != null && GameManager.instance.CurrentState == GameState.GAME_PLAY)
        {
            float zPos = Random.Range(33.5f, 55.5f);
            Instantiate(itemPrefabs[0], new Vector3(0, 3, zPos), Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }
        StartCoroutine(Spawn_Item_Coroutine());
    }

    // public void SetStage(int stage, int round)
    // {
    //     stageSettings = Resources.Load<StageSettingSO>("StageSettings" + stage);

    //     monsterPrefabs = stageSettings.enemies;
    //     bossPrefab = stageSettings.boss;
    //     enemyRatio = new int[][] { stageSettings.enemyRatio1, stageSettings.enemyRatio2, stageSettings.enemyRatio3 };
    //     totalEnemies = stageSettings.totalEnemies[round - 1];
    //     spawnRate = stageSettings.spawnRatios[round - 1];

    //     spawnedEnemyRatio = new int[monsterPrefabs.Length];
    // }
    
    public void SetStage(int stageNumber)
    {
        stageSettings = Resources.Load<StageSettingSO>(StageSettings.GetStageFromNumber(stageNumber));
    }

    public void SetRound(int round)
    {
        monsterPrefabs = stageSettings.enemies;
        bossPrefab = stageSettings.boss;
        enemyRatio = new int[][] { stageSettings.enemyRatio1, stageSettings.enemyRatio2, stageSettings.enemyRatio3 };
        totalEnemies = stageSettings.totalEnemies[round - 1];
        spawnRate = stageSettings.spawnRatios[round - 1];

        spawnedEnemyRatio = new int[monsterPrefabs.Length];

        enemyCount = 0;
        roundTotalKillCount = 0;
    }

    public void AddRoundKillCount()
    {
        roundTotalKillCount++;
        if (roundTotalKillCount >= totalEnemies)
        {
            GameProgressManager.Instance.CompleteRound();
        }
    }
}
