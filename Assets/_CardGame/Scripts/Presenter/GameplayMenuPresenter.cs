using System;
using UniRx;
using Zenject;

public class GameplayMenuPresenter : IInitializable, IDisposable
{
    private readonly GameplayMenuView view;
    private readonly GameModel model;
    private readonly SaveService saveService;
    private readonly GamePresenter gamePresenter;
    private readonly ISceneLoader sceneLoader;
    private readonly PromptService promptService;

    public GameplayMenuPresenter(GameplayMenuView view,
        GameModel model,
        SaveService saveService,
        GamePresenter gamePresenter,
        ISceneLoader sceneLoader,
        PromptService promptService)
    {
        this.view = view;
        this.model = model;
        this.saveService = saveService;
        this.gamePresenter = gamePresenter;
        this.sceneLoader = sceneLoader;
        this.promptService = promptService;

        Initialize();
    }

    public void Initialize()
    {
        // Bind model to UI
        model.Score.Subscribe(score => view.SetScore(score)).AddTo(view);
        model.Combo.Subscribe(combo => view.SetCombo(combo)).AddTo(view);
        model.Moves.Subscribe(moves => view.SetMoves(moves)).AddTo(view);

        // Back button logic
        view.OnBackClicked
            .Subscribe(_ => GoToMainMenu())
            .AddTo(view);

        // Save button logic
        view.OnSaveClicked
            .Subscribe(_ => SaveGame())
            .AddTo(view);

        this.gamePresenter.onCompleteBoardSetUp.Subscribe(_ => this.view.SetLoadingPanel(false, 2f)).AddTo(view);
        this.gamePresenter.onGameOver
           .Subscribe(_ =>
            {
                promptService.Show("Game Over!");
                view.ChangeSaveButtonInteractibity(false);
                Observable.Timer(System.TimeSpan.FromSeconds(2))
                          .Subscribe(__ => GoToMainMenu())
                          .AddTo(view);
            })
            .AddTo(view);
    }

    private void SaveGame()
    {
        promptService.Show("Saved Game!");
        saveService.SaveGame(model.ToSaveData());
    }

    private void GoToMainMenu()
    {
        sceneLoader.LoadScene(0);
    }

    public void Dispose()
    {
        // Clean up if needed
    }
}