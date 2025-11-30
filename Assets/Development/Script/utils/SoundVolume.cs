// 사운드 카테고리 및 항목별 최종 볼륨을 계산하는 유틸리티 클래스
public static class SoundVolume
{
    // 공통 계산: 항목 비율 × 카테고리 볼륨 (카테고리 볼륨에는 이미 마스터 볼륨 포함)
    private static float Final(float categoryScalar, float ratio)
        => ratio * categoryScalar;

    public static class BGM
    {
        public static float MainTheme => Final(SoundSettingSO.BGMVolume, VolumeRatioSO.Instance.bgmMainTheme);
        public static float BattleTheme => Final(SoundSettingSO.BGMVolume, VolumeRatioSO.Instance.bgmBattleTheme);
    }

    public static class SFX
    {
        // Player
        public static float PlayerAttack => Final(SoundSettingSO.SFXVolume, VolumeRatioSO.Instance.sfxPlayerAttack);
        public static float PlayerHit => Final(SoundSettingSO.SFXVolume, VolumeRatioSO.Instance.sfxPlayerHit);
        public static float PlayerDie => Final(SoundSettingSO.SFXVolume, VolumeRatioSO.Instance.sfxPlayerDie);
        public static float PlayerLevelUp => Final(SoundSettingSO.SFXVolume, VolumeRatioSO.Instance.sfxPlayerLevelUp);

        // Monster
        public static float MonsterSpawn => Final(SoundSettingSO.SFXVolume, VolumeRatioSO.Instance.sfxMonsterSpawn);
        public static float MonsterAttack => Final(SoundSettingSO.SFXVolume, VolumeRatioSO.Instance.sfxMonsterAttack);
        public static float MonsterHit => Final(SoundSettingSO.SFXVolume, VolumeRatioSO.Instance.sfxMonsterHit);
        public static float MonsterDie => Final(SoundSettingSO.SFXVolume, VolumeRatioSO.Instance.sfxMonsterDie);

        // Item
        public static float ItemDrop => Final(SoundSettingSO.SFXVolume, VolumeRatioSO.Instance.sfxItemDrop);
        public static float ItemCollect => Final(SoundSettingSO.SFXVolume, VolumeRatioSO.Instance.sfxItemCollect);
    }

    public static class UI
    {
        public static float ButtonClick => Final(SoundSettingSO.UIVolume, VolumeRatioSO.Instance.uiButtonClick);
        public static float Notification => Final(SoundSettingSO.UIVolume, VolumeRatioSO.Instance.uiNotification);
    }
}
