using UnityEngine;
using VContainer;

public class TurretController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float maxRotationAngle = 60f; // Максимальний кут повороту (щоб не стріляла назад)
    
    private IInputService _inputService;
    private IGameStateService _gameState;

    [Inject]
    public void Construct(IInputService inputService, IGameStateService gameState)
    {
        _inputService = inputService;
        _gameState = gameState;
    }

    private void Update()
    {
        // Турель крутиться тільки коли гра триває
        if (_gameState == null || _gameState.CurrentState != GameState.Playing) return;

        // 1. Отримуємо напрямок від гравця (відносно машини)
        Vector3 aimDirection = _inputService.GetAimDirection();

        // 2. Рахуємо цільовий кут обертання відносно локальної осі (машини)
        float targetAngle = Mathf.Atan2(aimDirection.x, aimDirection.z) * Mathf.Rad2Deg;
        
        // Обмежуємо кут, щоб турель не розверталася назад
        targetAngle = Mathf.Clamp(targetAngle, -maxRotationAngle, maxRotationAngle);

        // 3. Створюємо цільовий поворот навколо осі Y
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

        // 4. Плавно обертаємо турель
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}