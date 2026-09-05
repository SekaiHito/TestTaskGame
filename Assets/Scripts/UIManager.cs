using UnityEngine;
using VContainer;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    private IGameStateService _gameState;

    [Inject]
    public void Construct(IGameStateService gameState)
    {
        _gameState = gameState;
        _gameState.OnStateChanged += UpdateUI;
    }

    private void Start()
    {
        // Примусово оновлюємо інтерфейс при запуску сцени
        if (_gameState != null)
        {
            UpdateUI(_gameState.CurrentState);
        }
    }

    private void UpdateUI(GameState state)
    {
        startScreen.SetActive(state == GameState.Init);
        winScreen.SetActive(state == GameState.Win);
        loseScreen.SetActive(state == GameState.Lose);
    }

    private void OnDestroy()
    {
        if (_gameState != null) _gameState.OnStateChanged -= UpdateUI;
    }
}