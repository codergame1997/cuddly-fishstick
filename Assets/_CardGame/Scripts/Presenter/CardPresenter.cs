using UniRx;
using UnityEngine;
using System;
using DG.Tweening;

public class CardPresenter : IDisposable
{
    public CardModel model { get; }
    public CardView view { get; }

    private CompositeDisposable disposables = new CompositeDisposable();

    public IObservable<Unit> OnRequestFlip => view.OnClicked;

    public CardPresenter(CardModel model, CardView view, Sprite frontSprite)
    {
        this.model = model;
        this.view = view;
        if (view != null)
        {
            view.SetFrontSprite(frontSprite);
            view.SetInteractable(model.IsInteractable.Value);
            view.OnClicked.Subscribe(_ => HandleClick()).AddTo(disposables);
            model.State.Subscribe(OnModelStateChanged).AddTo(disposables);
            model.IsInteractable.Subscribe(b => view.SetInteractable(b)).AddTo(disposables);
        }
    }

    private void HandleClick()
    {
        if (!model.IsInteractable.Value || model.State.Value != CardState.FaceDown) return;
        // presenter will notify higher level (game presenter). We'll expose click event
        // via the OnRequestFlip in the creator, but here we'll simply forward via view's subject.
    }

    private void OnModelStateChanged(CardState state)
    {
        if (view == null) return;
        switch (state)
        {
            case CardState.FaceDown:
                view.Flip(false);
                break;
            case CardState.FaceUp:
                view.Flip(true);
                break;
            case CardState.Matched:
                // play matched animation (scale out)
                view.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f).OnComplete(() => view.gameObject.SetActive(false));
                break;
        }
    }

    public void Dispose()
    {
        disposables.Dispose();
    }
}