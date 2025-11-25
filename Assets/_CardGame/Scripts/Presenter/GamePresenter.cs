using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using System;
using DG.Tweening;

public class GamePresenter : IDisposable
{
    private readonly GameModel model;
    private readonly BoardView boardView;
    private readonly LayoutConfig layout;
    private readonly List<CardPresenter> presenters = new List<CardPresenter>();
    private readonly Subject<Unit> comparisonQueue = new Subject<Unit>();
    private readonly CompositeDisposable disposables = new CompositeDisposable();

    private bool processingComparison = false;

    public GamePresenter(GameModel model, BoardView boardView, LayoutConfig layout, IEnumerable<CardData> cardDatas)
    {
        this.model = model;
        this.boardView = boardView;
        this.layout = layout;

        SetupBoard(cardDatas);

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

        // Create models
        var cardModels = new List<CardModel>();
        for (int i = 0; i < idList.Count; i++)
        {
            cardModels.Add(new CardModel(idList[i], i));
        }
        this.model.ResetModel(cardModels);

        // Instantiate views and presenters
        boardView.Clear();
        for (int i = 0; i < cardModels.Count; i++)
        {
            var cardView = boardView.CreateCard();
            var cardData = cardDatas.First(d => d.id == cardModels[i].id);
            var cardPresenter = new CardPresenter(cardModels[i], cardView, cardData.frontSprite);

            // Connect clicks
            cardView.OnClicked.Subscribe(_ => OnCardClicked(cardModels[i])).AddTo(disposables);
            presenters.Add(cardPresenter);
        }

        // TODO: Responsive layout logic
    }

    private void OnCardClicked(CardModel cardModel)
    {
        if (!cardModel.IsInteractable.Value || cardModel.State.Value != CardState.FaceDown) return;

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
            model.Combo.Value += 1;
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

        model.Moves.Value += 1;

        processingComparison = false;
    }

    public void Dispose()
    {
        foreach (var p in presenters) p.Dispose();
        disposables.Dispose();
    }
}