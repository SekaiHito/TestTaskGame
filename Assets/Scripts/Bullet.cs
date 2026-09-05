using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    [SerializeField] private float lifeTime = 2f; // Через скільки секунд куля зникне, якщо нікуди не влучить
    
    private Action<Bullet> _returnToPool;
    private float _timer;

    // Ініціалізація при діставанні з пулу
    public void Init(Action<Bullet> returnAction)
    {
        _returnToPool = returnAction;
        _timer = lifeTime;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _returnToPool?.Invoke(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Якщо об'єкт має здоров'я - завдаємо йому 10 одиниць шкоди
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(10);
        }
        
        // Незалежно від того, влучили у ворога чи в стіну - повертаємо кулю в пул
        _returnToPool?.Invoke(this);
    }
}