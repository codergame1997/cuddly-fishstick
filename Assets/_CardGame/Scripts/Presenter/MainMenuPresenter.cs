using UniRx;
using UnityEngine;
using System;

public class MainMenuPresenter
{
    private readonly MainMenuView view;
    private readonly SaveService saveService;
    private readonly LayoutDatabase layoutDatabase;
    private readonly ISceneLoader sceneLoader;

    public MainMenuPresenter(MainMenuView view, SaveService saveService, LayoutDatabase layoutDatabase, ISceneLoader sceneLoader)
    {
        this.view = view;
        this.saveService = saveService;
        this.layoutDatabase = layoutDatabase;
        this.sceneLoader = sceneLoader;
    }

    public void Initialize()
    {
        bool hasSave = saveService.HasSave();
        view.SetLoadButtonInteractable(hasSave);

        view.playButton.OnClickAsObservable()
            .Subscribe(_ => StartNewGame())
            .AddTo(view);

        view.loadButton.OnClickAsObservable()
            .Subscribe(_ => LoadSavedGame())
            .AddTo(view);

        view.exitButton.OnClickAsObservable()
            .Subscribe(_ => Application.Quit())
            .AddTo(view);
    }

    private void StartNewGame()
    {
        var randomLayout = layoutDatabase.GetRandomLayout();
        GameContext.SelectedLayout = randomLayout; // store selection globally
        GameContext.IsLoadGame = false;
        sceneLoader.LoadScene("2. GamePlay");
    }

    private void LoadSavedGame()
    {
        GameContext.IsLoadGame = true;
        sceneLoader.LoadScene("2. GamePlay");
    }
}