using UnityEngine;
using VContainer;

[RequireComponent(typeof(HealthComponent))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private float detectionDistance = 15f;
    [SerializeField] private int damageToCar = 20;

    private IGameStateService _gameState;
    private PlayerRegistry _registry; // Зберігаємо сам реєстр, а не координати
    private HealthComponent _health;
    private bool _isChasing = false;
    private Animator _animator;


    [Inject]
    public void Construct(IGameStateService gameState, PlayerRegistry registry)
    {
        _gameState = gameState;
        _registry = registry;
    }

    private void Awake()
    {
        _health = GetComponent<HealthComponent>();
        _health.OnDeath += HandleDeath;
        _animator = GetComponent<Animator>(); // Отримуємо аніматор
    }

   private void Update()
    {
        if (_gameState == null || _gameState.CurrentState != GameState.Playing) return;
        
        Transform target = _registry?.PlayerTransform;
        if (target == null) return;

        if (!_isChasing)
        {
            if (Vector3.Distance(transform.position, target.position) < detectionDistance)
            {
                _isChasing = true;
                if (_animator != null) _animator.SetBool("IsRunning", true);
            }
        }
        else
        {
            // Цільова позиція бере X та Z від машини, але зберігає поточний Y стікмена
            Vector3 targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
            
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            transform.LookAt(targetPos);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var carHealth = other.GetComponentInParent<IDamageable>();
            
            if (carHealth != null)
            {
                carHealth.TakeDamage(damageToCar);
            }
            
            HandleDeath();
        }
    }

    private void HandleDeath() => Destroy(gameObject);
    private void OnDestroy() { if (_health != null) _health.OnDeath -= HandleDeath; }
}