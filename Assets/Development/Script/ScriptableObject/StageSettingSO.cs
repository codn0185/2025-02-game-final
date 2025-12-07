using UnityEngine;

[CreateAssetMenu(fileName = "StageSettings", menuName = "Stage setting")]
public class StageSettingSO : ScriptableObject
{
    [Header("Monsters")]
    public GameObject[] enemies;

    public int[] totalEnemies = { 50, 70, 100 };
    public int[] enemyRatio1 = { 90, 10, 0 };
    public int[] enemyRatio2 = { 60, 30, 10 };
    public int[] enemyRatio3 = { 40, 40, 20 };

}