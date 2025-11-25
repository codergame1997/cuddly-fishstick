using System.Collections.Generic;
using UniRx;
using System.Linq;

public class GameModel
{
    public ReactiveCollection<CardModel> Cards { get; } = new ReactiveCollection<CardModel>();
    public ReactiveProperty<int> Score { get; } = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> Moves { get; } = new ReactiveProperty<int>(0);
    public ReactiveProperty<bool> IsBusy { get; } = new ReactiveProperty<bool>(false);
    public ReactiveProperty<int> Combo { get; } = new ReactiveProperty<int>(0);

    private LayoutConfig selectedLayout;

    private readonly List<CardModel> tempFaceUp = new List<CardModel>();

    public void ResetModel()
    {
        Cards.Clear();
        Score.Value = 0;
        Moves.Value = 0;
        Combo.Value = 0;
        IsBusy.Value = false;
    }

    public void SetLayout(LayoutConfig layoutConfig)
    {
        selectedLayout = layoutConfig;
    }

    public void AddCard(CardModel card)
    {
        Cards.Add(card);
    }

    public List<CardModel> GetFaceUpInteractableCards()
    {
        tempFaceUp.Clear();
        foreach (var c in Cards)
            if (c.State.Value == CardState.FaceUp && c.IsInteractable.Value)
                tempFaceUp.Add(c);
        return tempFaceUp;
    }

    #region SAVE_SYSTEM

    public GameSaveData ToSaveData()
    {
        var data = new GameSaveData();
        data.score = Score.Value;
        data.combo = Combo.Value;
        data.moves = Moves.Value;
        data.layoutConfig = selectedLayout;

        foreach (var card in Cards)
        {
            data.cards.Add(new CardSaveData
            {
                id = card.id,
                uniqueInstanceId = card.uniqueInstanceId,
                state = card.State.Value.ToString(),
                isInteractable = card.IsInteractable.Value
            });
        }

        return data;
    }

    #endregion
}