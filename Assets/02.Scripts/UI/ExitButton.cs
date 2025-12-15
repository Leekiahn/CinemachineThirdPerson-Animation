using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    private Button exitButton;

    private void Awake()
    {
        exitButton = GetComponent<Button>();
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonPressed);
        }
    }

    /// <summary>
    /// 애플리케이션 종료
    /// </summary>
    public void OnExitButtonPressed()
    {
        Application.Quit();
    }
}
