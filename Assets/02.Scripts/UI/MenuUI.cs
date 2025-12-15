using RPGCharacterAnims;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject menuPanel;

    [Header("Input")]
    private PlayerInputHandler inputHandler;
    private PlayerCameraController cameraController;

    private bool isMenuOpen = false;

    private void Awake()
    {
        // 입력 핸들러가 할당되지 않은 경우 자동으로 찾기
        if (inputHandler == null)
        {
            inputHandler = FindAnyObjectByType<PlayerInputHandler>();
        }

        if(cameraController == null)
        {
            cameraController = inputHandler.gameObject.GetComponent<PlayerCameraController>();
        }
    }

    private void Start()
    {
        // 초기 상태: 메뉴 비활성화
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
            isMenuOpen = false;
        }
    }

    private void OnEnable()
    {
        if (inputHandler != null)
        {
            inputHandler.OnESCInputChanged += ToggleMenu;
        }
    }

    private void OnDisable()
    {
        if (inputHandler != null)
        {
            inputHandler.OnESCInputChanged -= ToggleMenu;
        }
    }

    /// <summary>
    /// 메뉴 활성화/비활성화 토글
    /// </summary>
    public void ToggleMenu()
    {
        if (menuPanel == null) return;

        isMenuOpen = !isMenuOpen;
        menuPanel.SetActive(isMenuOpen);

        // 메뉴가 열리면 시간 정지, 닫히면 재개
        Time.timeScale = isMenuOpen ? 0f : 1f;
        // 카메라 컨트롤 활성화/비활성화
        if (cameraController != null)
        {
            cameraController.enabled = !isMenuOpen;
        }

        // 커서 표시 설정
        Cursor.visible = isMenuOpen;
        Cursor.lockState = isMenuOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }
}