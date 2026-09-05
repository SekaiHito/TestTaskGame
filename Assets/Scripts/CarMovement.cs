using UnityEngine;
using VContainer; // Обов'язково для [Inject]

public class CarMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    
    private IGameStateService _gameState;
    private bool _canMove = false;

    // VContainer знайде цей метод і передасть сюди потрібні сервіси під час Instantiate
    [Inject]
    public void Construct(IGameStateService gameState)
    {
        _gameState = gameState;
        _gameState.OnStateChanged += HandleStateChanged;
    }

    private void HandleStateChanged(GameState state)
    {
        // Машина їде тільки в стані Playing
        _canMove = (state == GameState.Playing);
    }

    private void Update()
    {
        if (!_canMove) return;

        // Рух вперед 
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnDestroy()
    {
        // Завжди відписуємось від подій, щоб не було помилок при рестарті
        if (_gameState != null)
        {
            _gameState.OnStateChanged -= HandleStateChanged;
        }
    }
}