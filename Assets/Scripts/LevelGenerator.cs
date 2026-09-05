using UnityEngine;
using VContainer;
using VContainer.Unity; 

public class LevelGenerator : MonoBehaviour
{
    [Header("Налаштування Дороги")]
    [SerializeField] private GameObject groundChunkPrefab;
    [SerializeField] private int levelLengthInChunks = 15;
    [SerializeField] private int extraChunksAfterFinish = 3;
    [SerializeField] private float chunkLength = 20f;

    [Header("Налаштування Ворогів")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemiesCount = 20;
    [SerializeField] private float roadWidth = 8f; // Допустима ширина спавну (вліво-вправо)
    [SerializeField] private float safeZoneLength = 30f; // Відстань від старту без ворогів

    [Header("Фініш")]
    [SerializeField] private GameObject finishLinePrefab;

    private IObjectResolver _container;

    [Inject]
    public void Construct(IObjectResolver container)
    {
        _container = container;
    }

    public void GenerateLevel()
    {
        ClearLevel();

        // 1. Спавнимо дорогу: основна довжина + "хвіст" для гальмування
        int totalChunks = levelLengthInChunks + extraChunksAfterFinish;
        for (int i = 0; i < totalChunks; i++)
        {
            Vector3 spawnPos = new Vector3(0, 0, i * chunkLength);
            Instantiate(groundChunkPrefab, spawnPos, Quaternion.identity, transform);
        }

        // 2. Фініш ставимо рівно на кінці основної дистанції 
        float finishZPosition = levelLengthInChunks * chunkLength;
        Vector3 finishPos = new Vector3(0, 0, finishZPosition);
        _container.Instantiate(finishLinePrefab, finishPos, Quaternion.identity, transform);

        // 3. Спавн ворогів (тільки до фінішу, щоб за ним була чиста зона)
        for (int i = 0; i < enemiesCount; i++)
        {
            float randomX = Random.Range(-roadWidth / 2f, roadWidth / 2f);
            // Обмеження, щоб вороги не спавнилися за фінішем
            float randomZ = Random.Range(safeZoneLength, finishZPosition - 10f); 
            
            Vector3 spawnPos = new Vector3(randomX, 0, randomZ);
            _container.Instantiate(enemyPrefab, spawnPos, Quaternion.Euler(0, 180, 0), transform);
        }
    }

    public void ClearLevel()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}