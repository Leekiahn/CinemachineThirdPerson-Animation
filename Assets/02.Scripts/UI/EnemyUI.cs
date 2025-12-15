using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : BaseUI
{
    protected override void OnEnable()
    {
        base.OnEnable();
        if (characterStats != null)
        {
            characterStats.onUIVisibilityChanged.AddListener(SetUIVisibility);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (characterStats != null)
        {
            characterStats.onUIVisibilityChanged.RemoveListener(SetUIVisibility);
        }
    }

    private void Start()
    {
        // 초기 UI 비활성화
        SetUIVisibility(false);
    }

    private void SetUIVisibility(bool isVisible)
    {
        healthBar.gameObject.SetActive(isVisible);
    }
}
