using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Налаштування")]
    [SerializeField] private Vector3 offset = new Vector3(0, 5f, -8f); // Висота і віддаленість
    [SerializeField] private float smoothSpeed = 10f; // Швидкість згладжування
    
    private Transform _target;

    // Цей метод ми викличемо з Bootstrapper'а, коли створимо авто
    public void SetTarget(Transform target)
    {
        _target = target;
        
        // Одразу переміщуємо камеру на старт без затримок
        transform.position = _target.position + offset;
        transform.LookAt(_target);
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        // Плавний рух до цілі
        Vector3 desiredPosition = _target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        
        // Камера завжди дивиться на авто
        transform.LookAt(_target);
    }
}