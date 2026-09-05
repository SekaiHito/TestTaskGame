using System;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage);
    event Action OnDeath;
}

public class HealthComponent : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;

    [SerializeField] private int currentHealth; 

    public event Action OnDeath;
    public event Action<float, float> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        
        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }
}