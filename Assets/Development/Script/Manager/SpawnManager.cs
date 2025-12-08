using System.Collections;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] public StageSettingSO stageSettings;
    private static StageSettingSO Settings => Instance.stageSettings;
    private int[] enemyRatio = { 40, 30, 30 };
    private int totalEnemies = 50;
    int[] spawnedEnemies;
    int enemyCount = 0;
    private GameObject[] monsterPrefabs;
    public GameObject[] itemPrefabs;
    public GameObject spawnPortal;

    protected override void Awake()
    {
        base.Awake();
        if (stageSettings == null)
        {
            stageSettings = Resources.Load<StageSettingSO>("Stage1Settings");
        }
        monsterPrefabs = Settings.enemies;
        enemyRatio = Settings.enemyRatio1;
        totalEnemies = Settings.totalEnemies[0];

        spawnedEnemies = new int[monsterPrefabs.Length];

        StartCoroutine(Spawn_Monster_Coroutine());
        // StartCoroutine(Spawn_Item_Coroutine());
    }

    void Update()
    {

    }
    IEnumerator Spawn_Monster_Coroutine()
    {
        // while (true)
        // {
        //     if (GameManager.Instance.CurrentState != GameState.GAME_PLAY)
        //     {
        //         continue;
        //     }

        //     Round.RoundData rd = GameManager.Instance.CurrentRoundData;
        //     if (rd != null && rd.round >= 1)
        //     {
        //         float xPos = Random.Range(-4.0f, 4.0f);
        //         float zPos = Random.Range(33.5f, 55.5f);
        //         Instantiate(monsterPrefab, new Vector3(xPos, 0.32f, zPos), Quaternion.Euler(0, 180, 0));

        //         float wait = Mathf.Max(0.01f, Random.Range(1f, 1.5f) / rd.mob_spawn_rate);
        //         yield return new WaitForSeconds(wait);
        //     }
        //     else
        //     {
        //         yield return new WaitForSeconds(1f);
        //     }
        // }

        if (GameManager.instance != null && GameManager.instance.CurrentState == GameState.GAME_PLAY && totalEnemies > enemyCount)
        {
            if (!spawnPortal.activeSelf)
                spawnPortal.SetActive(true);

            Round.RoundData rd = GameManager.instance.CurrentRoundData;
            float xPos = Random.Range(-4.0f, 4.0f);
            // float zPos = Random.Range(33.5f, 55.5f);
            Vector3 spawnPosition = new Vector3(xPos, 0.32f, 50f);
            if (enemyCount % 10 > 3)
            {

                for (int i = 0; i < monsterPrefabs.Length; i++)
                {
                    if (enemyRatio[i] > spawnedEnemies[i] / enemyCount * 100)
                    {
                        Instantiate(monsterPrefabs[i], spawnPosition, Quaternion.Euler(0, 180, 0));
                        break;
                    }
                }

            }
            else
                Instantiate(monsterPrefabs[Random.Range(0, monsterPrefabs.Length)], spawnPosition, Quaternion.Euler(0, 180, 0));

            enemyCount++;
            float wait = Mathf.Max(0.01f, Random.Range(1f, 1.5f) / rd.mob_spawn_rate);
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
}
