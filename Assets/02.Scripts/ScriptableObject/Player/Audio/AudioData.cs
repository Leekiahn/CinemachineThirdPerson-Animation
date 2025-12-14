using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAudioData", menuName = "Scriptable Object/Player/Audio Data")]
public class AudioData : ScriptableObject
{
    [Header("FootStep Sounds")]
    public AudioClip[] walkFootStepSound;
    public AudioClip[] sprintFootStepSound;
    public AudioClip[] diveRollFootStepSound;
    public AudioClip[] landFootStepSound;
    public AudioClip[] hitEnemySound;

    [Header("Voice Sounds")]
    public AudioClip[] diveRollVoice;
    public AudioClip[] landVoice;
    public AudioClip[] attackVoice;
    public AudioClip[] hitVoice;
    public AudioClip[] deadVoice;

    [Header("Item Sounds")]
    public AudioClip healSound;

    [Header("BGM Sounds")]
    public AudioClip BGM;

    public AudioClip GetRandomClip(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return null;
        int randomIndex = Random.Range(0, clips.Length);
        return clips[randomIndex];
    }
}
