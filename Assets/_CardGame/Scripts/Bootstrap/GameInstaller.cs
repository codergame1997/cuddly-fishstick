using Zenject;
using UnityEngine;

public class GameInstaller : MonoInstaller
{
    public BoardView boardView;
    public LayoutConfig layoutConfig;
    public CardData[] cardPool;

    public override void InstallBindings()
    {
        // Core model & services
        Container.Bind<GameModel>().AsSingle();
        Container.Bind<SaveService>().AsSingle();

        // Scene references
        Container.Bind<BoardView>().FromInstance(boardView).AsSingle();
        Container.Bind<LayoutConfig>().FromInstance(layoutConfig).AsSingle();
        Container.Bind<CardData[]>().FromInstance(cardPool).AsSingle();

        // Presenter
        Container.Bind<GamePresenter>().AsSingle().NonLazy();

        // Hook for pause/quit saving
        Container.BindInterfacesAndSelfTo<GameLifecycleHandler>().AsSingle().NonLazy();
    }
}