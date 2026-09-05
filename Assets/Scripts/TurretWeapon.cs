using UnityEngine;
using UnityEngine.Pool;
using VContainer;

public class TurretWeapon : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform firePoint; // Порожній об'єкт на кінці дула турелі
    [SerializeField] private float fireRate = 0.2f; // Затримка між пострілами

    private IGameStateService _gameState;
    private ObjectPool<Bullet> _pool;
    private float _fireTimer;

    [Inject]
    public void Construct(IGameStateService gameState)
    {
        _gameState = gameState;
    }

    private void Awake()
    {
        // Налаштування вбудованого пулу
        _pool = new ObjectPool<Bullet>(
            createFunc: () => Instantiate(bulletPrefab),
            actionOnGet: bullet => 
            {
                bullet.gameObject.SetActive(true);
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = firePoint.rotation;
                bullet.Init(b => _pool.Release(b)); // Передаємо метод для повернення
            },
            actionOnRelease: bullet => bullet.gameObject.SetActive(false),
            actionOnDestroy: bullet => Destroy(bullet.gameObject),
            defaultCapacity: 20,
            maxSize: 50
        );
    }

    private void Update()
    {
        if (_gameState == null || _gameState.CurrentState != GameState.Playing) return;

        _fireTimer += Time.deltaTime;
        if (_fireTimer >= fireRate)
        {
            _fireTimer = 0f;
            _pool.Get(); // Дістаємо кулю з пулу - відбувається постріл
        }
    }
}