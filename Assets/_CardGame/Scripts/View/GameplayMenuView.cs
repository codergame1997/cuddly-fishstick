using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using System;

public class GameplayMenuView : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button saveButton;
    [SerializeField] private Button backButton;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private TMP_Text movesText;
    [SerializeField] private GameObject loadingPanel;

    public IObservable<Unit> OnBackClicked => backButton.OnClickAsObservable();
    public IObservable<Unit> OnSaveClicked => saveButton.OnClickAsObservable();

    public void SetScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void SetCombo(int combo)
    {
        comboText.text = $"Combo: {combo}";
    }

    public void SetMoves(int moves)
    {
        movesText.text = $"Moves: {moves}";
    }

    public void SetLoadingPanel(bool value, float delay)
    {
        Observable
            .Timer(TimeSpan.FromSeconds(delay))
            .ObserveOnMainThread()
            .Subscribe(_ =>
            {
                loadingPanel.SetActive(value);
            })
            .AddTo(this);
    }

    public void ChangeSaveButtonInteractibity(bool value)
    {
        saveButton.interactable = value;
    }
}