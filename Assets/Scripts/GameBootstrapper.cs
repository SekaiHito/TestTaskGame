using VContainer;
using VContainer.Unity;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameBootstrapper : IInitializable, ITickable
{
    private readonly IGameStateService _gameState;
    private readonly LevelGenerator _levelGenerator;
    private readonly CameraFollow _cameraFollow;
    private readonly Func<Vector3, Quaternion, GameObject> _carFactory;
    private readonly PlayerRegistry _playerRegistry;

    private GameObject _currentCar;

    [Inject]
    public GameBootstrapper(
        IGameStateService gameState, 
        LevelGenerator levelGenerator, 
        CameraFollow cameraFollow,
        Func<Vector3, Quaternion, GameObject> carFactory,
        PlayerRegistry playerRegistry) 
    {
        _gameState = gameState;
        _levelGenerator = levelGenerator;
        _cameraFollow = cameraFollow;
        _carFactory = carFactory;
        _playerRegistry = playerRegistry;
    }

    public void Initialize()
    {
        SpawnLevelAndCar();
    }

    private void SpawnLevelAndCar()
    {
        _levelGenerator.GenerateLevel();
        
        if (_currentCar != null) UnityEngine.Object.Destroy(_currentCar);
        
        _currentCar = _carFactory(Vector3.zero, Quaternion.identity);
        _playerRegistry.PlayerTransform = _currentCar.transform; 
        
        _cameraFollow.SetTarget(_currentCar.transform);
        _gameState.ChangeState(GameState.Init); // Повертаємо стан на екран старту
    }

    public void Tick()
    {
        bool isTapped = false;
        
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) isTapped = true;
        else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame) isTapped = true;

        if (!isTapped) return; // Якщо не клікнули - нічого не робимо

        // Логіка для стану Init (Старт гри)
        if (_gameState.CurrentState == GameState.Init)
        {
            _gameState.ChangeState(GameState.Playing);
        }
        // Логіка для станів Win / Lose (Рестарт гри)
        else if (_gameState.CurrentState == GameState.Win || _gameState.CurrentState == GameState.Lose)
        {
            SpawnLevelAndCar();
        }
    }
}