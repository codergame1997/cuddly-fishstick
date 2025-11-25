using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class MainMenuView : MonoBehaviour
{
    public Button playButton;
    public Button loadButton;
    public Button exitButton;

    public void SetLoadButtonInteractable(bool active)
    {
        loadButton.interactable = active;
    }
}