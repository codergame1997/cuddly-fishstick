using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveData
{
    public int score;
    public int combo;
    public int moves;
    public LayoutConfig layoutConfig;
    public List<CardSaveData> cards = new List<CardSaveData>();
}

[Serializable]
public class CardSaveData
{
    public string id;
    public int uniqueInstanceId;
    public string state; // "FaceDown", "FaceUp", "Matched"
    public bool isInteractable;
}