using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool
{
    private readonly Transform parent;
    private readonly List<AudioSource> pool = new List<AudioSource>();
    private readonly int initialSize;

    public AudioSourcePool(Transform parent, int initialSize = 8)
    {
        this.parent = parent;
        this.initialSize = initialSize;

        for (int i = 0; i < initialSize; i++)
        {
            pool.Add(CreateNewSource());
        }
    }

    private AudioSource CreateNewSource()
    {
        GameObject go = new GameObject("PooledAudioSource");
        go.transform.SetParent(parent);
        AudioSource source = go.AddComponent<AudioSource>();
        source.playOnAwake = false;
        return source;
    }

    public AudioSource Get()
    {
        foreach (var src in pool)
        {
            if (!src.isPlaying)
                return src;
        }

        // Expand pool if needed
        var newSource = CreateNewSource();
        pool.Add(newSource);
        return newSource;
    }
}