/* 
빈 프로필 UI
- 새 프로필 생성을 위한 빈 슬롯
- 클릭 시 이름 입력 UI 표시
- 이름 입력 후 확인 버튼 클릭 시 프로필 생성 및 활성화
*/

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EmptyProfileUI : MonoBehaviour
{
    [Header("Name Input UI")]
    [SerializeField] private GameObject nameInputUI;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TextMeshProUGUI warningText;

    private void OnSlotClicked()
    {
        // 프로필 생성 가능 여부 확인
        if (!ProfileManager.CanCreateNewProfile())
        {
            Debug.LogWarning("ProfileManager: Cannot create more profiles. Maximum limit reached.");
            return;
        }

        // 이름 입력 UI 표시
        nameInputUI.SetActive(true);

        // 입력 필드 초기화 및 포커스
        nameInputField.text = "";
        nameInputField.ActivateInputField();
    }

    private void OnConfirmButtonClicked()
    {
        // 경고 텍스트 숨기기
        warningText.gameObject.SetActive(false);

        string inputName = nameInputField.text.Trim();

        // 이름이 비어있는지 확인하여 경고 표시
        if (string.IsNullOrEmpty(inputName))
        {
            warningText.gameObject.SetActive(true);
            return;
        }

        // 이름이 입력된 경우: 프로필 생성 및 활성화
        if (ProfileManager.Instance.TryCreateAndActiveProfile())
        {
            Debug.Log($"ProfileManager: Created and activated new profile (UID: {ProfileManager.Instance.UID}, name: {inputName}).");

            // 생성된 프로필에 이름 설정
            ProfileManager.Instance.SetUserName(inputName);

            // 게임 메인 씬으로 전환
            SceneManager.LoadScene(Scene.GameLobby);
        }
        else
        {
            Debug.LogWarning("ProfileManager: Failed to create and activate new profile.");

            // 이름 입력 UI 숨기기
            nameInputUI.SetActive(false);
        }
    }
}
