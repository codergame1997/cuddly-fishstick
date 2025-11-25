using UnityEngine;
using System.Collections.Generic;

public class BoardView : MonoBehaviour
{
    public RectTransform container; // area that holds cards
    public GameObject cardPrefab;

    private readonly List<CardView> liveViews = new List<CardView>();

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