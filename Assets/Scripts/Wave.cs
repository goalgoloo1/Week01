using UnityEngine;

[System.Serializable]
public class Wave : MonoBehaviour
{
    public int enemyCount; // 적의 수
    public int allyCount; // 아군의 수
    public float spawnInterval; // 적 스폰 간격
    public float waveDelay; // 다음 웨이브까지의 대기 시간
}