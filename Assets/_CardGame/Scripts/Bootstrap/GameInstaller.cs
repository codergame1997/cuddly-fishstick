using Zenject;
using UnityEngine;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private BoardView boardView;
    [SerializeField] private LayoutConfig layoutConfig;
    [SerializeField] private CardData[] cardPool;
    [SerializeField] private AudioService audioService;

    public override void InstallBindings()
    {
        // Core model & services
        Container.Bind<GameModel>().AsSingle();
        Container.Bind<SaveService>().AsSingle();

        // Scene references
        Container.Bind<BoardView>().FromInstance(boardView).AsSingle();
        Container.Bind<LayoutConfig>().FromInstance(layoutConfig).AsSingle();
        Container.Bind<CardData[]>().FromInstance(cardPool).AsSingle();

        // Audio service in scene
        Container.Bind<AudioService>().FromInstance(audioService).AsSingle();

        // Presenter
        Container.Bind<GamePresenter>().AsSingle().NonLazy();

        // Hook for pause/quit saving
        Container.BindInterfacesAndSelfTo<GameLifecycleHandler>().AsSingle().NonLazy();
    }
}