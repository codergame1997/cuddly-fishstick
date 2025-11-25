using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using System;

public class GameplayMenuView : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button saveButton;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private TMP_Text movesText;

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
}