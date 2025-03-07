using UnityEngine;
using System.Collections;

public class Enemy : Character
{
    public GameObject bulletPrefab; // 총알 프리팹
    public Transform firePoint; // 총알 발사 위치
    private float fireRate = 3f; // 발사 주기
    private bool isFiring = false; // 발사 중인지 여부
    private float moveTimer; // 자유 이동 시간 타이머
    private Vector3 randomDirection; // 자유 이동 방향
    public ParticleSystem deathParticle;

    private GameObject player; // 플레이어
    private GameObject[] allies; // 아군 배열
    private GameObject currentTarget; // 현재 타겟 (플레이어 또는 아군)

    private void Start()
    {
        hp = 1;
        movespeed = 10f;
        player = GameObject.FindGameObjectWithTag("Player"); // 플레이어 찾기
        moveTimer = 0f;
        ChooseRandomDirection();
    }

    void Update()
    {
        if (player == null) return;

        // 아군 찾기
        allies = GameObject.FindGameObjectsWithTag("Ally");

        // 가장 가까운 타겟 찾기
        FindClosestTarget();

        if (currentTarget != null)
        {
            float distance = Vector3.Distance(transform.position, currentTarget.transform.position);

            if (distance > 20f) // 거리가 20f을 넘으면 자유롭게 이동
            {
                FreeMove();
            }
            else if (distance <= 20f && distance > 10f) // 거리가 20f과 10f 사이면 타겟 추적
            {
                ChaseTarget();
            }
            else if (distance <= 10f) // 거리가 10f보다 작으면 총알 발사
            {
                RotateTowardsTarget();
                if (!isFiring)
                {
                    StartCoroutine(FireAtTarget());
                }
            }
        }

        // 체력이 0 이하이면 사망
        if (hp < 1)
        {
            GameManager gm = GameObject.FindFirstObjectByType<GameManager>();
            TriggerDeath();
            Destroy(gameObject);
        }
    }

    // 가장 가까운 타겟 찾기 (플레이어 또는 아군)
    void FindClosestTarget()
    {
        float closestDistance = Mathf.Infinity;
        currentTarget = null;

        // 플레이어와의 거리 계산
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < closestDistance)
            {
                closestDistance = distanceToPlayer;
                currentTarget = player;
            }
        }

        // 아군과의 거리 계산
        if (allies != null && allies.Length > 0)
        {
            foreach (GameObject ally in allies)
            {
                if (ally != null)
                {
                    float distanceToAlly = Vector3.Distance(transform.position, ally.transform.position);
                    if (distanceToAlly < closestDistance)
                    {
                        closestDistance = distanceToAlly;
                        currentTarget = ally;
                    }
                }
            }
        }
    }

    // 타겟을 향해 회전
    void RotateTowardsTarget()
    {
        if (currentTarget != null)
        {
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            transform.up = -direction; // 타겟을 향해 회전
        }
    }

    // 타겟을 향해 이동
    void ChaseTarget()
    {
        if (currentTarget != null)
        {
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            transform.position += direction * movespeed * Time.deltaTime;
            transform.up = -direction; // 타겟을 향해 회전
        }
    }

    // 총알 발사
    IEnumerator FireAtTarget()
    {
        isFiring = true;
        yield return new WaitForSeconds(2f); // 2초 대기 후 발사

        while (currentTarget != null && Vector3.Distance(transform.position, currentTarget.transform.position) <= 10f)
        {
            GameObject flash = transform.Find("Flash").gameObject;
            if (flash)
            {
                flash.SetActive(true);
                yield return new WaitForSeconds(0.1f);
                flash.SetActive(false);
            }
            else
            {
                Debug.Log("적 플래시 못찾음");
            }

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().SetDirection((currentTarget.transform.position - transform.position).normalized);
            bullet.GetComponent<Bullet>().from = gameObject.tag;
            bullet.transform.rotation = transform.rotation;
            yield return new WaitForSeconds(fireRate);
        }
        isFiring = false;
    }

    // 자유롭게 이동
    void FreeMove()
    {
        moveTimer += Time.deltaTime;

        // 일정 시간마다 방향 변경
        if (moveTimer > 3f)
        {
            ChooseRandomDirection();
            moveTimer = 0f;
        }

        transform.position += randomDirection * (movespeed * 0.5f) * Time.deltaTime;
        transform.Rotate(0, 0, Random.Range(-60f, 60f) * Time.deltaTime);
    }

    // 랜덤 방향 선택
    void ChooseRandomDirection()
    {
        randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
    }

    // 사망 처리
    void TriggerDeath()
    {
        if (deathParticle != null)
        {
            ParticleSystem death = Instantiate(deathParticle, transform.position, Quaternion.identity);
            death.Play();
            Destroy(death.gameObject, 2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && collision.gameObject.GetComponent<Bullet>().from != gameObject.tag)
        {
            hp--;
            collision.gameObject.SetActive(false);
        }
    }
}