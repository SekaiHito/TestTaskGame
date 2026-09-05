using UnityEngine;
using VContainer;

[RequireComponent(typeof(HealthComponent))]
public class PlayerStateController : MonoBehaviour
{
    private IGameStateService _gameState;
    private HealthComponent _health;

    [Inject]
    public void Construct(IGameStateService gameState)
    {
        _gameState = gameState;
    }

    private void Awake()
    {
        _health = GetComponent<HealthComponent>();
        _health.OnDeath += HandleDeath;
    }

    private void HandleDeath()
    {
        _gameState.ChangeState(GameState.Lose);
    }

    private void OnDestroy()
    {
        if (_health != null) _health.OnDeath -= HandleDeath;
    }
}