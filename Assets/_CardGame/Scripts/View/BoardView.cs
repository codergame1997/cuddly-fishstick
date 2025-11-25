using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoardView : MonoBehaviour
{
    [SerializeField] private RectTransform container; // area that holds cards
    [SerializeField] private GameObject cardPrefab;

    private GridLayoutGroup gridLayout;

    private readonly List<CardView> liveViews = new List<CardView>();

    void Awake()
    {
        if (gridLayout == null)
        {
            gridLayout = container.GetComponent<GridLayoutGroup>();
            if (gridLayout == null)
                gridLayout = container.gameObject.AddComponent<GridLayoutGroup>();
        }
    }

    // Arrange cards in grid as per layout
    public void ArrangeCardsInGrid(LayoutConfig layout)
    {
        if (gridLayout == null)
            gridLayout = container.GetComponent<GridLayoutGroup>();

        Vector2 parentSize = container.rect.size;

        float cardWidth = (parentSize.x - (layout.columns - 1) * layout.spacing.x) / layout.columns;
        float cardHeight = (parentSize.y - (layout.rows - 1) * layout.spacing.y) / layout.rows;

        gridLayout.cellSize = new Vector2(cardWidth, cardHeight);
        gridLayout.spacing = layout.spacing;
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = layout.columns;
        gridLayout.childAlignment = TextAnchor.MiddleCenter;
    }

    public void Clear()
    {
        foreach (var v in liveViews) if (v) Destroy(v.gameObject);
        liveViews.Clear();
    }

    public CardView CreateCard()
    {
        var go = Instantiate(cardPrefab, container);
        var cv = go.GetComponent<CardView>();
        liveViews.Add(cv);
        return cv;
    }

    public IReadOnlyList<CardView> LiveViews => liveViews;
}