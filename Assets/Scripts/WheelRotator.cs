using UnityEngine;
using VContainer;

public class WheelRotator : MonoBehaviour
{
    [SerializeField] private Transform[] wheels; 
    [SerializeField] private float rotationSpeed = 500f; // Швидкість обертання
    
    private IGameStateService _gameState;

    [Inject]
    public void Construct(IGameStateService gameState)
    {
        _gameState = gameState;
    }

    private void Update()
    {
        // Крутимо тільки тоді, коли ми їдемо
        if (_gameState == null || _gameState.CurrentState != GameState.Playing) return;

        float rotation = rotationSpeed * Time.deltaTime;
        
        foreach (var wheel in wheels)
        {
            // Space.Self означає, що колесо крутиться навколо власної осі, а не світової
            wheel.Rotate(Vector3.right, rotation, Space.Self); 
        }
    }
}
