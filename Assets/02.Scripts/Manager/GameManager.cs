using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        BGMManager.Instance.PlayBGM(BGMManager.Instance.audioData.BGM);
    }
}
