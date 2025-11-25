using UnityEngine;

public enum SoundType
{
    Flip,
    Match,
    Mismatch,
    GameOver
}

[System.Serializable]
public class SoundData
{
    public SoundType type;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
}