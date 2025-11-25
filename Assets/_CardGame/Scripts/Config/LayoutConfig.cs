using UnityEngine;

[CreateAssetMenu(menuName = "CardMatchGame/LayoutConfig")]
public class LayoutConfig : ScriptableObject
{
    public int columns = 4;
    public int rows = 3;

    // spacing between cards
    public Vector2 spacing = new Vector2(10, 10);

    // utility
    public int TotalCards => rows * columns;
}