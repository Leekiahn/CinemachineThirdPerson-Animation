using UnityEngine;

public class PlayerStats : CharacterStats
{
    private PlayerInputHandler playerInputHandler;
    private PlayerCameraController playerCameraController;

    protected override void Awake()
    {
        base.Awake();
        playerInputHandler = GetComponent<PlayerInputHandler>();
        playerCameraController = GetComponent<PlayerCameraController>();
    }

    protected override void Die()
    {
        base.Die();
        animator.SetBool(hashDie, true);
        playerInputHandler.enabled = false; // 플레이어 입력 비활성화
        playerCameraController.enabled = false; // 카메라 컨트롤 비활성화
    }
}
