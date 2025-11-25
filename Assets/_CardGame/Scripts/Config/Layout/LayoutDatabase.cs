using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardMatchGame/LayoutDatabase")]
public class LayoutDatabase : ScriptableObject
{
    public List<LayoutConfig> layouts = new List<LayoutConfig>();

    public LayoutConfig GetRandomLayout()
    {
        if (layouts == null || layouts.Count == 0)
            return null;

        int index = Random.Range(0, layouts.Count);
        return layouts[index];
    }
}