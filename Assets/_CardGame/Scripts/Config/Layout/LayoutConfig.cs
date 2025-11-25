using UnityEngine;

[System.Serializable]
public class LayoutConfig
{
    public int rows = 3;
    public int columns = 4;

    // spacing between cards
    public Vector2 spacing = new Vector2(10, 10);

    // utility
    public int TotalCards => rows * columns;
}