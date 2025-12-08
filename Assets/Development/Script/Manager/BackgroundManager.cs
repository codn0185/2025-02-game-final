/* 
배경이 뒤로 이동하며 인피니트 러너 효과 구현
    - 배경을 구획별 프리팹을 제작
    - 랜덤한 구획이 소환되며 뒤로 이동
    - 뒤로 이동한 배경은 일정 거리 이상 이동하면 파괴
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : Singleton<BackgroundManager>
{
    [SerializeField] private GameObject LeftWall;
    [SerializeField] private GameObject RightWall;
    [SerializeField] private GameObject Floor;

    [SerializeField] private GameObject[] Stage1BackgroundPrefabs;
    [SerializeField] private GameObject[] Stage2BackgroundPrefabs;
    [SerializeField] private GameObject[] Stage3BackgroundPrefabs;
    [SerializeField] private Vector3 SpawnPosition = new(0, 0, 60);
    [SerializeField] private Vector3 DespawnPosition = new(0, 0, -20);
    [SerializeField] private float MoveSpeed = 3f; // 배경 이동 속도 
    [SerializeField] private float SpawnSpeed = 3f; // 배경 소환 주기 (초)

    public GameObject[][] StageBackgroundPrefabs { get; private set; }
    private List<GameObject> SpawnedBackgroundGameObjects = new();
    private Coroutine spawnCoroutine;
    private int CurrentStage => GameProgressManager.Instance.CurrentStage;
    private float offset = 0f;

    protected override void Awake()
    {
        base.Awake();

        StageBackgroundPrefabs = new GameObject[][]
        {
            Stage1BackgroundPrefabs,
            Stage2BackgroundPrefabs,
            Stage3BackgroundPrefabs
        };
    }

    void Update()
    {
        if (GameProgressManager.Instance.CurrentState != GameProgressState.Playing)
            return;
        ScrollTexture();
        ScrollBackgrounds();
        RemoveOutOfScreenBackgrounds();
    }

    private void ScrollTexture()
    {
        offset += Time.deltaTime * 0.1f;
        if (offset > 1f) offset -= 1f;
        LeftWall.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, -offset);
        RightWall.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, -offset);
        Floor.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, -offset);
    }

    private void ScrollBackgrounds()
    {
        foreach (GameObject bg in SpawnedBackgroundGameObjects)
        {
            bg.transform.Translate(MoveSpeed * Time.deltaTime * Vector3.back);
        }
    }

    private void SpawnBackground(GameObject backgroundPrefab)
    {
        // Debug.Log("Spawning Background: " + backgroundPrefab.name);
        GameObject bg = Instantiate(backgroundPrefab, SpawnPosition, Quaternion.identity);
        SpawnedBackgroundGameObjects.Add(bg);
    }

    private void SpawnRandomBackgroundForStage(int stage)
    {
        if (stage < 1 || stage > StageBackgroundPrefabs.Length) return;

        GameObject[] backgrounds = StageBackgroundPrefabs[stage - 1];
        int randomIndex = Random.Range(0, backgrounds.Length);
        SpawnBackground(backgrounds[randomIndex]);
    }

    private void RemoveOutOfScreenBackgrounds()
    {
        for (int i = 0; i < SpawnedBackgroundGameObjects.Count; i++)
        {
            GameObject bg = SpawnedBackgroundGameObjects[i];
            if (bg.transform.position.z <= DespawnPosition.z)
            {
                Destroy(bg);
                SpawnedBackgroundGameObjects.RemoveAt(i);
                i--;
                // Debug.Log("Background Removed");
            }
        }
    }

    private IEnumerator SpawnBackgroundRoutine()
    {
        while (true)
        {
            Debug.Log("Spawning Background");
            SpawnRandomBackgroundForStage(CurrentStage);
            yield return new WaitForSeconds(SpawnSpeed);
        }
    }

    public void StartSpawningBackgrounds()
    {
        if (spawnCoroutine != null) return;
        spawnCoroutine = StartCoroutine(SpawnBackgroundRoutine());
        Debug.Log("Background Spawning Started");
    }

    public void StopSpawningBackgrounds()
    {
        if (spawnCoroutine == null) return;
        StopCoroutine(spawnCoroutine);
        spawnCoroutine = null;
        Debug.Log("Background Spawning Stopped");
    }

    public void ClearAllBackgrounds()
    {
        foreach (GameObject bg in SpawnedBackgroundGameObjects)
        {
            Destroy(bg);
        }
        SpawnedBackgroundGameObjects.Clear();
        StopSpawningBackgrounds();
        Debug.Log("All Backgrounds Cleared");
    }
}