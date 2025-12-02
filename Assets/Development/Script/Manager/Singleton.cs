using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // 싱글톤 인스턴스 찾기
                _instance = FindAnyObjectByType<T>();
                // 없으면 새로 생성
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject($"{typeof(T).Name} (Singleton)");
                    _instance = singletonObject.AddComponent<T>();
                }
                // 씬 전환 시 파괴 방지
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}