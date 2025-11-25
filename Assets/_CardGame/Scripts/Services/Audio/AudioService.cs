using UnityEngine;

public class AudioService : MonoBehaviour, IAudioService
{
    [SerializeField] private SoundDatabase soundDatabase;
    [SerializeField] private int poolSize = 8;

    private AudioSourcePool sourcePool;

    void Awake()
    {
        sourcePool = new AudioSourcePool(transform, poolSize);
    }

    public void Play(SoundType type)
    {
        var sound = soundDatabase.GetSound(type);
        if (sound == null || sound.clip == null)
        {
            Debug.LogWarning($"Sound {type} not found in database!");
            return;
        }

        AudioSource src = sourcePool.Get();
        src.clip = sound.clip;
        src.volume = sound.volume;
        src.loop = false;
        src.Play();
    }

    public void StopAll()
    {
        foreach (var src in GetComponentsInChildren<AudioSource>())
        {
            src.Stop();
        }
    }
}