using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject enemyPrefab;
    public GameObject emergencyKitPrefab;
    public GameObject redZonePrefab;
    public GameObject redZoneBoundsPrefab;
    public GameObject patientPrefab;
    public GameObject allyPrefab;

    [Header("References")]
    private GameObject _Player;
    [Header("Spawn Settings")]
    public float spawnRadius = 6f;
    public int initialEnemyCount = 10; // 초기 적 스폰 수
    public int initialAllyCount = 6; // 초기 아군 스폰 수
    public float minDistanceBetweenObjects = 2f; // 오브젝트 간 최소 거리
    [Header("Map Settings")]
    public float mapSizeX = 20f; 
    public float mapSizeY = 10f;
    public float charBoundX = 44f;
    public float charBoundY = 30f;

    [Header("Wave Settings")]
    public List<Wave> waves; // 웨이브 목록
    public int currentWave = 0; // 현재 웨이브
    public bool isWaveActive = false; // 웨이브가 진행 중인지 여부

    GameManager gm;
    GameObject player; 




    void Start()
    {
        _Player = Resources.Load<GameObject>("Prefabs/Player");
        gm = GameObject.FindFirstObjectByType<GameManager>();

        //StartCoroutine(SpawnEnemies());
        // StartCoroutine(SpawnEmergencyKits());
        //StartCoroutine(SpawnDeadZones());
        //StartCoroutine(SpawnAlly());
        //StartCoroutine(SpawnPatients());

        StartCoroutine(StartWaves());

        //SpawnInitialEnemies(); // 초기 적 스폰
        //SpawnInitialAllies(); // 초기 아군 스폰

        player = GameObject.FindFirstObjectByType<Player>().gameObject;
    }

    // 웨이브 시작
    IEnumerator StartWaves()
    {
        while (currentWave < waves.Count)
        {
            Wave wave = waves[currentWave];
            Debug.Log($"Wave {currentWave + 1} 시작!");

            // 적과 아군 스폰
            isWaveActive = true;
            yield return StartCoroutine(SpawnWave(wave));

            // 웨이브 종료 대기
            isWaveActive = false;
            Debug.Log($"Wave {currentWave + 1} 종료!");

            // 다음 웨이브까지 대기
            yield return new WaitForSeconds(wave.waveDelay);

            currentWave++;
        }

        Debug.Log("모든 웨이브 클리어!");
    }

    // 적과 아군 스폰
    IEnumerator SpawnWave(Wave wave)
    {
        // 적 스폰
        for (int i = 0; i < wave.enemyCount; i++)
        {
            Vector3 spawnPos = GetValidSpawnPosition();
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        // 아군 스폰
        for (int i = 0; i < wave.allyCount; i++)
        {
            Vector3 spawnPos = GetValidSpawnPosition();
            Instantiate(allyPrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    // 유효한 스폰 위치 계산
    Vector3 GetValidSpawnPosition()
    {
        Vector3 spawnPos;
        bool isValidPosition;
        int attempts = 0;

        do
        {
            spawnPos = GetRandomSpawnPosition();
            isValidPosition = CheckDistanceFromOtherObjects(spawnPos);
            attempts++;
        } while (!isValidPosition && attempts < 100);

        return spawnPos;
    }

    // 랜덤 스폰 위치 생성
    Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            Random.Range(-spawnRadius, spawnRadius),
            0
        );
    }

    // 다른 오브젝트와의 거리 확인
    bool CheckDistanceFromOtherObjects(Vector3 position)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");

        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(position, enemy.transform.position) < minDistanceBetweenObjects)
            {
                return false;
            }
        }

        foreach (GameObject ally in allies)
        {
            if (Vector3.Distance(position, ally.transform.position) < minDistanceBetweenObjects)
            {
                return false;
            }
        }

        return true;
    }



    void SpawnInitialEnemies()
    {
        for (int i = 0; i < initialEnemyCount; i++)
        {
            Vector3 spawnPos = GetRandomSpawnPositionEnemy();
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }

    // 초기 아군 스폰
    void SpawnInitialAllies()
    {
        for (int i = 0; i < initialAllyCount; i++)
        {
            Vector3 spawnPos = GetRandomSpawnPositionAlly();
            Instantiate(allyPrefab, spawnPos, Quaternion.identity);
        }
    }
    // Enemy Spawning: 20 units away from Player
    IEnumerator SpawnEnemies()
    {
        while (!gm.isgameover)
        {
            yield return new WaitForSeconds(3f);
            if (gm.isgameover) break;
            if (_Player != null)
            {
                Vector3 spawnPos = GetRandomPositionFarFromPlayer(20f);
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            }
        }
        yield return 0;
    }

    IEnumerator SpawnAlly()
    {
        while (!gm.isgameover)
        {
            yield return new WaitForSeconds(3f);
            if (gm.isgameover) break;
            if (_Player != null)
            {
                Vector3 spawnPos = GetRandomPositionNearPlayer(20f);
                Instantiate(allyPrefab, spawnPos, Quaternion.identity);

            }
        }
    }

    // Emergency Kit Spawning: Within 20 units of Player
    IEnumerator SpawnEmergencyKits()
    {
        while (!gm.isgameover)
        {
            yield return new WaitForSeconds(5f);
            if (_Player != null)
            {
                Vector3 spawnPos = GetRandomPositionNearPlayer(20f);
                Instantiate(emergencyKitPrefab, spawnPos, Quaternion.identity);
            }
        }
        yield return 0;
    }

    // Dead Zone Spawning: Within 20 units of Player
    IEnumerator SpawnDeadZones()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            if (_Player != null)
            {
                Vector3 spawnPos = GetRandomPositionNearPlayer(15f);
                GameObject redZoneBound = Instantiate(redZoneBoundsPrefab, spawnPos, Quaternion.identity);
                GameObject redZone = Instantiate(redZonePrefab, spawnPos, Quaternion.identity);
                redZone.GetComponent<RedZoneFire>().redZoneBound = redZoneBound;
            }
        }
    }

    

    // Patient Spawning: After 1.5 seconds, then every 3 seconds
    IEnumerator SpawnPatients()
    {
        yield return new WaitForSeconds(1.5f);
        while (!gm.isgameover)
        {
            yield return new WaitForSeconds(4f);
            Vector3 spawnPos = GetRandomScreenPosition();
            Instantiate(patientPrefab, spawnPos, Quaternion.identity);
        }
        yield return 0;
    }

    // Get a random position at least 'minDistance' away from the player
    Vector3 GetRandomPositionFarFromPlayer(float minDistance)
    {
        Vector3 spawnPos;
        do
        {
            spawnPos = new Vector3(
                Random.Range(-mapSizeX, mapSizeX),
                Random.Range(-mapSizeY, mapSizeY),
                0);
        } while (Vector3.Distance(spawnPos, _Player.transform.position) < minDistance);

        return spawnPos;
    }

    // Get a random position within 'maxDistance' of the player
    Vector3 GetRandomPositionNearPlayer(float maxDistance)
    {
        Vector2 randomCircle = Random.insideUnitCircle * maxDistance;
        if (player)
        {

            float x = player.transform.position.x + randomCircle.x;
            float y = player.transform.position.y + randomCircle.y;
            x = Mathf.Clamp(x, -mapSizeX, mapSizeX);
            y = Mathf.Clamp(y, -mapSizeY, mapSizeY);
            return new Vector3(x, y, 0);
        }
        else 
        {
            return new Vector3(randomCircle.x, randomCircle.y, 0);
        } 
    }

    // Get a random position within the visible screen area
    Vector3 GetRandomScreenPosition()
    {
        return new Vector3(
            Random.Range(-mapSizeX, mapSizeX),
            Random.Range(-mapSizeY, mapSizeY),
            0);
    }

    Vector3 GetRandomSpawnPositionAlly()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(-charBoundX, charBoundX),
            Random.Range(-charBoundY, -15),
            0
        );
        return spawnPos;
    }

    Vector3 GetRandomSpawnPositionEnemy()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(-charBoundX, charBoundX),
            Random.Range(15, charBoundY),
            0
        );
        return spawnPos;
    }
}
