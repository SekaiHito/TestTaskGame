using UnityEngine;
using VContainer;

public class FinishTrigger : MonoBehaviour
{
    private IGameStateService _gameState;

    [Inject]
    public void Construct(IGameStateService gameState)
    {
        _gameState = gameState;
    }

    private void OnTriggerEnter(Collider other)
{
    Debug.Log($"Фініш торкнувся об'єкта: {other.name}, його тег: {other.tag}");

    if (other.CompareTag("Player"))
    {
        if (_gameState == null)
        {
            Debug.LogError("Помилка: _gameState дорівнює null! VContainer не зробив ін'єкцію.");
            return;
        }
        
        _gameState.ChangeState(GameState.Win);
    }
}
}