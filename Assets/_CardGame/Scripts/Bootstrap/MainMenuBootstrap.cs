using UnityEngine;

/// <summary>
/// This component will be replaced by Zenject bindings, but is kept for non-DI bootstrap setups.
/// It initializes the main menu components and presenter.
/// </summary>
public class MainMenuBootstrap : MonoBehaviour
{
    [SerializeField] private MainMenuView mainMenuView;
    [SerializeField] private LayoutDatabase layoutDatabase;

    private MainMenuPresenter presenter;
    private SaveService saveService;
    private SceneLoader sceneLoader;

    void Start()
    {
        saveService = new SaveService();
        sceneLoader = new SceneLoader();

        presenter = new MainMenuPresenter(mainMenuView, saveService, layoutDatabase, sceneLoader);
        presenter.Initialize();
    }
}