/* 
프로필에 반영되는 강화 및 해금 등을 관리하는 싱글톤 매니저 클래스
    - 강화: 마법 종류에 따라 기본 능력치가 다르므로 백분율로 관리
    - 스킬 해금: 해금된 스킬 목록 관리
 */

using System.Collections.Generic;

public enum MetaUpgradeType
{
    BaseHealth,
    BaseAttackPower,
    BaseAttackSpeed,
}

public class MetaManager : Singleton<MetaManager>
{
    protected override void Awake()
    {
        base.Awake();
    }
}