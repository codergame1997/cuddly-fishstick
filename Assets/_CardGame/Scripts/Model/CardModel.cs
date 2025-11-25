using UniRx;

public enum CardState
{
    FaceDown,
    Flipping,
    FaceUp,
    Matched
}

public class CardModel
{
    public string id; // ties to CardData.id
    public ReactiveProperty<CardState> State { get; } = new ReactiveProperty<CardState>(CardState.FaceDown);
    public ReactiveProperty<bool> IsInteractable { get; } = new ReactiveProperty<bool>(true);
    public int uniqueInstanceId; // to distinguish duplicates

    public CardModel(string id, int instanceId)
    {
        this.id = id;
        this.uniqueInstanceId = instanceId;
    }

    public void SetState(CardState s) => State.Value = s;

    public void Reset()
    {
        State.Value = CardState.FaceDown;
        IsInteractable.Value = true;
    }
}