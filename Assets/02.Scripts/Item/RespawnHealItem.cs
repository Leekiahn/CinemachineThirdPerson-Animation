using UnityEngine;

public class RespawnHealItem : MonoBehaviour
{
    [SerializeField] private GameObject healItem;
    [SerializeField] private float respawnTime = 5f;

    private float time = 0f;
    private void Update()
    {
        if (!healItem.activeSelf)
        {
            time += Time.deltaTime;
            if (time >= respawnTime)
            {
                healItem.SetActive(true);
                time = 0f;
            }
        }
    }
}
