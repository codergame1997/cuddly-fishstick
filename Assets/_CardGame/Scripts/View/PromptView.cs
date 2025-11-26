using UnityEngine;
using TMPro;
using UniRx;
using System;

public class PromptView : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text messageText;
    private readonly CompositeDisposable disposables = new CompositeDisposable();

    public void SubscribeToService(PromptService promptService)
    {
        promptService.OnPrompt.Subscribe(msg => ShowMessage(msg)).AddTo(this);
    }

    public void ShowMessage(string message, float duration = 2f)
    {
        panel.SetActive(true);
        messageText.text = message;

        // Hide after delay using UniRx Timer
        Observable
            .Timer(TimeSpan.FromSeconds(duration))
            .ObserveOnMainThread()
            .Subscribe(_ => panel.SetActive(false))
            .AddTo(disposables);
    }

    private void OnDestroy()
    {
        disposables.Dispose();
    }
}