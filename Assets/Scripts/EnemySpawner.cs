using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    [Header("General")]

    public float spawnInterval = 3f;   // �ʱ� �� ���� ����
    public float minSpawnInterval = 0.5f; // ���̵��� �ö� �� �ּ� ����
    public float difficultyIncreaseRate = 0.95f; // ���� ������ ���� (5%�� ����)
    public int maxEnemies = 30; // �ִ� �� ����
    private float currentSpawnInterval;
    private int currentEnemyCount = 0;

    public float mapSizeX = 5f;
    public float mapSizeY = 2f;

    void Start()
    {
        instance = this;
        currentSpawnInterval = spawnInterval;
        StartCoroutine(SpawnEnemiesOverTime());
    }

    IEnumerator SpawnEnemiesOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnInterval);

            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
                currentEnemyCount++;
            }

            // ���̵� ���� (���� ���� ���� �پ��)
            currentSpawnInterval *= difficultyIncreaseRate;
            if (currentSpawnInterval < minSpawnInterval)
                currentSpawnInterval = minSpawnInterval;
        }
    }

    public static void SpawnEnemy()
    {
        Vector3 spawnPosition = instance.GetRandomEdgePosition();
        ObjectPooling.GetEnemy(spawnPosition);
    }

    private Vector3 GetRandomEdgePosition()
    {
        int edge = Random.Range(0, 4); // 0: ����, 1: ������, 2: ����, 3: �Ʒ���
        float x = 0, y = 0;

        switch (edge)
        {
            case 0: // ����
                x = -mapSizeX;
                y = Random.Range(-mapSizeY, mapSizeY);
                break;
            case 1: // ������
                x = mapSizeX;
                y = Random.Range(-mapSizeY, mapSizeY);
                break;
            case 2: // ����
                x = Random.Range(-mapSizeX, mapSizeX);
                y = mapSizeY;
                break;
            case 3: // �Ʒ���
                x = Random.Range(-mapSizeX, mapSizeX);
                y = -mapSizeY;
                break;
        }

        return new Vector3(x, y, 0);
    }

    public static void EnemyDied()
    {
        instance.currentEnemyCount--;
    }
}
