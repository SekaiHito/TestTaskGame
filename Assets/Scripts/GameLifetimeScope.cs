using VContainer;
using VContainer.Unity;
using UnityEngine;

public class GameLifetimeScope : LifetimeScope
{
    // Сюди ми пізніше перетягнемо наші префаби, щоб VContainer міг їх створювати
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private UIManager uiManager;

    protected override void Configure(IContainerBuilder builder)
    {

        builder.Register<GameStateManager>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.RegisterEntryPoint<GameBootstrapper>();
        builder.RegisterComponent(levelGenerator);
        builder.RegisterComponent(cameraFollow);
        builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<PlayerRegistry>(Lifetime.Singleton);
        builder.RegisterComponent(uiManager);

        builder.RegisterFactory<Vector3, Quaternion, GameObject>(container =>
        {
            return (position, rotation) => 
            {
                return container.Instantiate(carPrefab, position, rotation);
            };
        }, Lifetime.Singleton);
    }
}