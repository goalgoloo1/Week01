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

    public float mapSizeX = 20f;
    public float mapSizeZ = 20f;

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
        float x = 0, z = 0;

        switch (edge)
        {
            case 0: // ����
                x = -mapSizeX;
                z = Random.Range(-mapSizeZ, mapSizeZ);
                break;
            case 1: // ������
                x = mapSizeX;
                z = Random.Range(-mapSizeZ, mapSizeZ);
                break;
            case 2: // ����
                x = Random.Range(-mapSizeX, mapSizeX);
                z = mapSizeZ;
                break;
            case 3: // �Ʒ���
                x = Random.Range(-mapSizeX, mapSizeX);
                z = -mapSizeZ;
                break;
        }

        return new Vector3(x, 0, z);
    }


    public static void EnemyDied()
    {
        instance.currentEnemyCount--;
    }
}
