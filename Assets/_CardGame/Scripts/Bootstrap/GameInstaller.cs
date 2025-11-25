using Zenject;

public class GameInstaller : MonoInstaller
{
    public BoardView boardView;
    public LayoutConfig layoutConfig;
    public CardData[] cardPool;

    public override void InstallBindings()
    {
        // Bind model as a simple instance
        Container.Bind<GameModel>().AsSingle();

        // Bind configuration & view instances provided in the scene
        Container.Bind<BoardView>().FromInstance(boardView).AsSingle();
        Container.Bind<LayoutConfig>().FromInstance(layoutConfig).AsSingle();
        Container.Bind<CardData[]>().FromInstance(cardPool).AsSingle();

        // Bind presenter with automatic construction & injection
        Container.Bind<GamePresenter>().AsSingle().NonLazy();
    }
}