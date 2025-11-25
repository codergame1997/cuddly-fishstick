using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "CardMatchGame/SoundDatabase")]
public class SoundDatabase : ScriptableObject
{
    public List<SoundData> sounds = new List<SoundData>();

    public SoundData GetSound(SoundType type)
    {
        return sounds.FirstOrDefault(s => s.type == type);
    }
}