using UnityEngine;

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