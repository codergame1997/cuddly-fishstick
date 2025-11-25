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

    private readonly List<CardModel> tempFaceUp = new List<CardModel>();

    public void ResetModel(IEnumerable<CardModel> cards)
    {
        Cards.Clear();
        foreach (var c in cards) Cards.Add(c);
        Score.Value = 0;
        Moves.Value = 0;
        Combo.Value = 0;
        IsBusy.Value = false;
    }

    public List<CardModel> GetFaceUpInteractableCards()
    {
        tempFaceUp.Clear();
        foreach (var c in Cards)
            if (c.State.Value == CardState.FaceUp && c.IsInteractable.Value)
                tempFaceUp.Add(c);
        return tempFaceUp;
    }
}