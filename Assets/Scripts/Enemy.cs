using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed = 3f; // 이동 속도
    private Transform player; // 플레이어 위치 저장
    public GameObject bulletPrefab; // 총알 프리팹
    public Transform firePoint; // 총알 발사 위치
    private float fireRate = 3f; // 발사 주기
    private bool isFiring = false; // 발사 중인지 여부
    private float moveTimer; // 자유 이동 시간 타이머
    private Vector3 randomDirection; // 자유 이동 방향

    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        moveTimer = 0f;
        ChooseRandomDirection();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        Debug.Log(distance);

        if (distance > 20f) //거리가 *f을 넘으면 자유로히움직임
        {
            FreeMove();
        }
        else if (distance <= 20f && distance > 10f) //거리가 *f과 *f사이면 플레이어추적
        {
            ChasePlayer();
        }
        else if (distance <= 10f) //거리가 *f보다 작으면 격발
        {
            RotateEnemy();
            if (!isFiring)
            {                                  
                StartCoroutine(FireAtPlayer());
            }
        }
    }

    void RotateEnemy() 
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.up = direction; // 플레이어를 향해 회전
    }
    // 자유롭게 이동 및 회전
    void FreeMove()
    {
        moveTimer += Time.deltaTime;

        // 일정 시간마다 방향 변경
        if (moveTimer > 3f)
        {
            ChooseRandomDirection();
            moveTimer = 0f;
        }

        transform.position += randomDirection * (speed * 0.5f) * Time.deltaTime;
        transform.Rotate(0, 0, Random.Range(-60f, 60f) * Time.deltaTime);
    }

    // 플레이어를 향해 이동
    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.up = direction; // 플레이어를 향해 회전
    }

    // 3초마다 플레이어에게 총알 발사
    IEnumerator FireAtPlayer()
    {
        isFiring = true;
        while (Vector3.Distance(transform.position, player.position) <= 20f)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().SetDirection((player.position - transform.position).normalized);
            yield return new WaitForSeconds(fireRate);
        }
        isFiring = false;
    }

    // 랜덤 방향 선택
    void ChooseRandomDirection()
    {
        randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
    }

    // 적 비활성화 및 오브젝트 풀로 반환
    void DeactivateEnemy()
    {
        StopAllCoroutines();
        ObjectPooling.ReturnEnemy(gameObject);
    }

    void OnDisable()
    {
        CancelInvoke();
        StopAllCoroutines();
    }
}
