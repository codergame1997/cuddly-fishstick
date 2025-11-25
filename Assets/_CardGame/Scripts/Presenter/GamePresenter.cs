using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using System;

public class GamePresenter : IDisposable
{
    private readonly GameModel model;
    private readonly BoardView boardView;
    private readonly LayoutConfig layout;
    private readonly List<CardPresenter> presenters = new List<CardPresenter>();
    private readonly Subject<Unit> comparisonQueue = new Subject<Unit>();
    private readonly CompositeDisposable disposables = new CompositeDisposable();

    private bool processingComparison = false;

    public GamePresenter(GameModel model,
        BoardView boardView,
        LayoutConfig layout,
        IEnumerable<CardData> cardDatas,
        GameSaveData gameSaveData)
    {
        this.model = model;
        this.boardView = boardView;
        this.layout = layout;

        if (gameSaveData != null)
        {
            RestoreFromSave(gameSaveData, cardDatas);
        }
        else
        {
            SetupBoard(cardDatas);
        }

        // Subscribe to queue
        comparisonQueue
            .ObserveOnMainThread()
            .Where(_ => !processingComparison)
            .Subscribe(_ => ProcessNextComparison())
            .AddTo(disposables);
    }

    private void SetupBoard(IEnumerable<CardData> cardDatas)
    {
        // Generate card model pairs from cardDatas to match layout.TotalCards (pairs)
        var pairsNeeded = layout.TotalCards / 2;
        var selected = cardDatas.Take(pairsNeeded).ToList();
        var idList = new List<string>();
        for (int i = 0; i < pairsNeeded; i++)
        {
            idList.Add(selected[i].id);
            idList.Add(selected[i].id);
        }

        // Shuffle
        var rnd = new System.Random();
        idList = idList.OrderBy(_ => rnd.Next()).ToList();

        this.model.ResetModel();

        // Instantiate views and presenters
        boardView.Clear();
        for (int i = 0; i < idList.Count; i++)
        {
            var cardView = boardView.CreateCard();
            var cardModel = new CardModel(idList[i], i);

            this.model.AddCard(cardModel);

            var cardData = cardDatas.First(d => d.id == cardModel.id);
            var cardPresenter = new CardPresenter(cardModel, cardView, cardData.frontSprite);

            // Connect clicks
            cardView.OnClicked.Subscribe(_ => OnCardClicked(cardModel)).AddTo(disposables);
            presenters.Add(cardPresenter);
        }

        boardView.ArrangeCardsInGrid(layout);
    }

    private void RestoreFromSave(GameSaveData save, IEnumerable<CardData> cardDatas)
    {
        boardView.Clear();
        this.model.ResetModel();

        // Spawn saved cards
        foreach (var cardSave in save.cards)
        {
            var cardView = boardView.CreateCard();
            var cardModel = new CardModel(cardSave.id, cardSave.uniqueInstanceId);
            this.model.AddCard(cardModel);

            if (System.Enum.TryParse(cardSave.state, out CardState parsed))
                cardModel.SetState(parsed);

            cardModel.IsInteractable.Value = cardSave.isInteractable;

            var cardData = cardDatas.First(x => x.id == cardSave.id);
            var cardPresenter = new CardPresenter(cardModel, cardView, cardData.frontSprite);

            // Connect clicks
            cardView.OnClicked.Subscribe(_ => OnCardClicked(cardModel)).AddTo(disposables);
            presenters.Add(cardPresenter);
        }

        // Arrange cards in board
        boardView.ArrangeCardsInGrid(layout);

        // Set scores
        model.Score.Value = save.score;
        model.Combo.Value = save.combo;
        model.Moves.Value = save.moves;
    }

    private void OnCardClicked(CardModel cardModel)
    {
        if (!cardModel.IsInteractable.Value || cardModel.State.Value != CardState.FaceDown) return;

        model.Moves.Value += 1;
        cardModel.SetState(CardState.FaceUp);

        // If we have two face-up cards, enqueue a comparison
        var faceUp = model.GetFaceUpInteractableCards();
        if (faceUp.Count >= 2)
        {
            comparisonQueue.OnNext(Unit.Default);
        }
    }

    private async void ProcessNextComparison()
    {
        processingComparison = true;

        // Find the first two face-up (interactable) cards
        var faceUp = model.GetFaceUpInteractableCards();
        if (faceUp.Count < 2)
        {
            processingComparison = false;
            return;
        }
        var a = faceUp[0];
        var b = faceUp[1];

        // Temporarily disable their interactable to avoid duplicate flips
        a.IsInteractable.Value = false;
        b.IsInteractable.Value = false;

        // Small delay so user sees the flip (non-blocking UI)
        await System.Threading.Tasks.Task.Delay(350);

        bool isMatch = a.id == b.id;
        if (isMatch)
        {
            a.SetState(CardState.Matched);
            b.SetState(CardState.Matched);
            model.Score.Value += 1;
            model.Combo.Value += 1;

            // combo bonus
            if (model.Combo.Value >= 2)
            {
                int bonus = CalculateBonus(model.Combo.Value);
                model.Score.Value += bonus;
                model.Combo.Value = 0;
            }
        }
        else
        {
            model.Combo.Value = 0;

            // Wait a bit so user sees the pair
            await System.Threading.Tasks.Task.Delay(550);

            a.SetState(CardState.FaceDown);
            b.SetState(CardState.FaceDown);
            a.IsInteractable.Value = true;
            b.IsInteractable.Value = true;
        }

        processingComparison = false;
    }

    // Helper method for dynamic bonus
    private int CalculateBonus(int combo)
    {
        // Exponential growth
        return Mathf.Min((int)Mathf.Pow(2, combo - 2), 10); // combo 2 => 1, combo 3 => 2, combo 4 => 4, etc.
    }

    public void Dispose()
    {
        foreach (var p in presenters) p.Dispose();
        disposables.Dispose();
    }
}