using UniRx;
using UnityEngine;
using System;
using DG.Tweening;
using Zenject;

public class CardPresenter : IInitializable, IDisposable
{
    public CardModel model { get; }
    public CardView view { get; }

    private Sprite _frontSprite;

    private CompositeDisposable disposables = new CompositeDisposable();

    public CardPresenter(CardModel model, CardView view, Sprite frontSprite)
    {
        this.model = model;
        this.view = view;
        this._frontSprite = frontSprite;
        Initialize();
    }

    public void Initialize()
    {
        if (view != null)
        {
            view.SetFrontSprite(_frontSprite);
            view.SetInteractable(model.IsInteractable.Value);
            view.OnClicked.Subscribe(_ => HandleClick()).AddTo(disposables);
            model.State.Subscribe(OnModelStateChanged).AddTo(disposables);
            model.IsInteractable.Subscribe(b => view.SetInteractable(b)).AddTo(disposables);
        }
    }

    private void HandleClick()
    {
        // Can add card specific click animation, sound, etc
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
                view.PlayMatchedAnimation();
                break;
        }
    }

    public void Dispose()
    {
        disposables.Dispose();
    }
}