using UnityEngine;

public class BGMManager : Singleton<BGMManager>
{
    private AudioSource audioSource;
    [SerializeField] private AudioData audioData;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    private void OnEnable()
    {
        PlayBGM(audioData.BGM);
    }
    public void PlayBGM(AudioClip clip)
    {
        if (audioSource.clip == clip) return;
        audioSource.clip = clip;
        audioSource.Play();
    }
    public void StopBGM()
    {
        audioSource.Stop();
    }
}
