using UniRx;

public class GameplayMenuPresenter
{
    private readonly GameplayMenuView view;
    private readonly GameModel model;
    private readonly SaveService saveService;
    private readonly GamePresenter gamePresenter;
    private readonly ISceneLoader sceneLoader;

    public GameplayMenuPresenter(GameplayMenuView view, GameModel model, SaveService saveService, GamePresenter gamePresenter, ISceneLoader sceneLoader)
    {
        this.view = view;
        this.model = model;
        this.saveService = saveService;
        this.gamePresenter = gamePresenter;
        this.sceneLoader = sceneLoader;

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
            .Subscribe(_ => saveService.SaveGame(model.ToSaveData()))
            .AddTo(view);

        this.gamePresenter.onCompleteBoardSetUp.Subscribe(_ => this.view.SetLoadingPanel(false, 2f)).AddTo(view);
    }

    private void GoToMainMenu()
    {
        sceneLoader.LoadScene(0);
    }
}