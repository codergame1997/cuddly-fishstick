using Zenject;
using UnityEngine;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private BoardView boardView;
    [SerializeField] private GameplayMenuView gameplayMenuView;
    [SerializeField] private CardData[] cardPool;
    [SerializeField] private AudioService audioService;
    [SerializeField] private SoundDatabase soundDatabase;

    public override void InstallBindings()
    {
        // Core model & services
        Container.Bind<GameModel>().AsSingle();
        Container.Bind<SaveService>().AsSingle();

        // Scene references
        Container.Bind<BoardView>().FromInstance(boardView).AsSingle();
        Container.Bind<GameplayMenuView>().FromInstance(gameplayMenuView).AsSingle();
        Container.Bind<CardData[]>().FromInstance(cardPool).AsSingle();
        Container.Bind<AudioService>().FromInstance(audioService).AsSingle();
        Container.Bind<SoundDatabase>().FromInstance(soundDatabase).AsSingle();

        // Presenters
        Container.Bind<GamePresenter>().AsSingle().NonLazy();
        Container.Bind<GameplayMenuPresenter>().AsSingle().NonLazy();

        // Create lifecycle handler for saving
        Container.BindInterfacesAndSelfTo<GameLifecycleHandler>().AsSingle().NonLazy();
    }
}