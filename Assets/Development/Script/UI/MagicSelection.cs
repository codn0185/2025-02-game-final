using UnityEngine;
using UnityEngine.UI;

public class MagicSelection : MonoBehaviour
{
    [Header("Magic Type Toggles")]
    [SerializeField] private Toggle earthToggle;
    [SerializeField] private Toggle iceToggle;
    [SerializeField] private Toggle fireToggle;
    [SerializeField] private Toggle lightningToggle;

    [Header("Start Button")]
    [SerializeField] private Button startButton;

    private MagicType selectedMagicType = MagicType.Earth; // 기본 선택

    void Start()
    {
        // 리스너 먼저 추가 (라디오 버튼 동작)
        earthToggle.onValueChanged.AddListener((isOn) => {
            if (isOn)
            {
                selectedMagicType = MagicType.Earth;
                SetOtherTogglesOff(earthToggle);
            }
        });
        
        iceToggle.onValueChanged.AddListener((isOn) => {
            if (isOn)
            {
                selectedMagicType = MagicType.Ice;
                SetOtherTogglesOff(iceToggle);
            }
        });
        
        fireToggle.onValueChanged.AddListener((isOn) => {
            if (isOn)
            {
                selectedMagicType = MagicType.Fire;
                SetOtherTogglesOff(fireToggle);
            }
        });
        
        lightningToggle.onValueChanged.AddListener((isOn) => {
            if (isOn)
            {
                selectedMagicType = MagicType.Lighting;
                SetOtherTogglesOff(lightningToggle);
            }
        });

        // 시작 버튼 리스너
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }

        // 기본 토글 선택 (Earth)
        earthToggle.isOn = true;
    }

    // 선택된 토글 외 나머지 토글 Off
    private void SetOtherTogglesOff(Toggle selectedToggle)
    {
        if (selectedToggle != earthToggle) earthToggle.isOn = false;
        if (selectedToggle != iceToggle) iceToggle.isOn = false;
        if (selectedToggle != fireToggle) fireToggle.isOn = false;
        if (selectedToggle != lightningToggle) lightningToggle.isOn = false;
    }

    private void OnStartButtonClicked()
    {
        // Player 찾아서 마법 타입 설정
        Player player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            player.SetMagicType(selectedMagicType);
            Debug.Log($"마법 타입 설정: {selectedMagicType}");
        }
        else
        {
            Debug.LogWarning("Player를 찾을 수 없습니다!");
        }

        // 여기에 게임 시작 로직 추가 가능
        // 예: GameManager.instance.StartGame();
    }

    // 외부에서 현재 선택된 마법 타입 가져오기
    public MagicType GetSelectedMagicType()
    {
        return selectedMagicType;
    }
}