using UnityEngine;
using UnityEngine.InputSystem;

// Інтерфейс, від якого залежатиме турель
public interface IInputService
{
    Vector3 GetAimDirection();
}

public class InputService : IInputService
{
    public Vector3 GetAimDirection()
    {
        Vector2 screenPos = Vector2.zero;
        bool isInteracting = false;

        // Читаємо мишку
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            screenPos = Mouse.current.position.ReadValue();
            isInteracting = true;
        }
        // Або читаємо тач
        else if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
        {
            screenPos = Touchscreen.current.touches[0].position.ReadValue();
            isInteracting = true;
        }

        // Якщо гравець не тримає палець/кнопку - турель дивиться просто вперед
        if (!isInteracting) return Vector3.forward;

        // Конвертуємо позицію на екрані у напрямок.
        // Центр екрану - це (0, 0), права частина - позитивний X, ліва - негативний.
        float halfWidth = Screen.width / 2f;
        float rawX = (screenPos.x - halfWidth) / halfWidth; // Значення від -1 до 1

        // Повертаємо вектор напрямку. Z завжди 1 (турель стріляє вперед), а X змінюється
        // Обмежуємо X, щоб турель не оберталася назад (наприклад, кут від -45 до 45 градусів)
        return new Vector3(Mathf.Clamp(rawX, -1f, 1f), 0, 1f).normalized;
    }
}