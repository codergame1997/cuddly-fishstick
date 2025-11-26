using UnityEngine;

/// <summary>
/// This component will be replaced by Zenject bindings, but is kept for non-DI bootstrap setups.
/// It initializes the core game components and presenters.
/// </summary>
public class GameBootstrap : MonoBehaviour
{
    [SerializeField] private BoardView boardView;
    [SerializeField] private PromptView promptView;
    [SerializeField] private GameplayMenuView gameplayMenuView;
    [SerializeField] private CardData[] cardPool; // assign all available card data assets
    [SerializeField] private AudioService audioService;
    [SerializeField] private SoundDatabase soundDatabase;

    private GamePresenter presenter;
    private GameplayMenuPresenter menuPresenter;
    private GameModel model;
    private SaveService saveService;
    private SceneLoader sceneLoader;
    private PromptService promptService;

    void Start()
    {
        model = new GameModel();
        saveService = new SaveService();
        sceneLoader = new SceneLoader();
        promptService = new PromptService();

        promptView.SubscribeToService(promptService);

        var savedData = saveService.LoadGame();
        presenter = new GamePresenter(model, boardView, cardPool, savedData, audioService);
        menuPresenter = new GameplayMenuPresenter(gameplayMenuView, model, saveService, presenter, sceneLoader, promptService);
    }

    void OnDestroy()
    {
        presenter?.Dispose();
    }

    /*
    void OnApplicationPause(bool pause)
    {
        if (pause && model != null)
            saveService.SaveGame(model.ToSaveData());
    }

    void OnApplicationQuit()
    {
        if (model != null)
            saveService.SaveGame(model.ToSaveData());
    }
    */
}