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

        // play button logic
        view.playButton.OnClickAsObservable()
            .Subscribe(_ => StartNewGame())
            .AddTo(view);

        // load button logic
        view.loadButton.OnClickAsObservable()
            .Subscribe(_ => LoadSavedGame())
            .AddTo(view);

        // exit button logic
        view.exitButton.OnClickAsObservable()
            .Subscribe(_ => Application.Quit())
            .AddTo(view);
    }

    private void StartNewGame()
    {
        var randomLayout = layoutDatabase.GetRandomLayout();
        GameContext.SelectedLayout = randomLayout; // store selection globally
        GameContext.IsLoadGame = false;
        sceneLoader.LoadScene(1);
    }

    private void LoadSavedGame()
    {
        GameContext.IsLoadGame = true;
        sceneLoader.LoadScene(1);
    }
}