/* 
SO로 각 메타 강화 정보 저장
- 최대 레벨
- 레벨 당 증가 비율
- 레벨 당 필요한 재화 (최대 레벨과 같은 크기의 배열)
- UI 이름
- UI 설명
- UI 이미지
 */

using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MetaUpgrade", menuName = "Settings/Meta Upgrade")]
public class MetaUpgradeSO : ScriptableObject
{
    [Header("Meta Upgrade Settings")]
    public MetaUpgradeType Type;
    public string Name;
    public string Description;
    public Sprite Icon;
    public int MaxLevel;
    public float IncreaseRatio;
    public int[] CostPerLevel; // 각 레벨 당 필요한 재화 배열

    public int GetCost(int level)
    {
        if (level < 0 || level >= CostPerLevel.Length)
        {
            return -1;
        }
        return CostPerLevel[level];
    }
}