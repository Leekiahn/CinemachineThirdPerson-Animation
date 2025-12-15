using UnityEngine;

public class PlayerUI : BaseUI
{
    private void Start()
    {
        if (characterStats == null)
        {
            characterStats = FindAnyObjectByType<PlayerStats>();
        }
    }
}
