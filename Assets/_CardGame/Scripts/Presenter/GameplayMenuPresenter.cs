using UniRx;

public class GameplayMenuPresenter
{
    private readonly GameplayMenuView view;
    private readonly GameModel model;
    private readonly SaveService saveService;

    public GameplayMenuPresenter(GameplayMenuView view, GameModel model, SaveService saveService)
    {
        this.view = view;
        this.model = model;
        this.saveService = saveService;

        // Bind model to UI
        model.Score.Subscribe(score => view.SetScore(score)).AddTo(view);
        model.Combo.Subscribe(combo => view.SetCombo(combo)).AddTo(view);
        model.Moves.Subscribe(moves => view.SetMoves(moves)).AddTo(view);

        // Save button logic
        view.OnSaveClicked
            .Subscribe(_ => saveService.SaveGame(model.ToSaveData()))
            .AddTo(view);
    }
}