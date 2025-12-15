using Unity.AI.Navigation;
using UnityEngine;

public class Environment : MonoBehaviour
{
    [SerializeField] private GameObject environmentPrefab;
    [SerializeField] private float environmentSize;

    private void Awake()
    {
        if (environmentPrefab != null)
        {
            for (int i = 0; i < 10; i++)
            {
                Instantiate(environmentPrefab, new Vector3(i + environmentSize, 0, 0), Quaternion.identity);
            }
        }

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
