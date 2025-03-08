using UnityEngine;
using System.Collections;

public class Ally : MonoBehaviour
{
    public int hp = 5; // 아군의 체력
    public GameObject patientPrefab; // 환자 프리팹
    public ParticleSystem damageParticle; // 데미지 입을 때 파티클
    public GameObject bulletPrefab; // 총알 프리팹
    public Transform firePoint; // 총알 발사 위치
    private float fireRate = 2f; // 발사 주기
    private bool isFiring = false; // 발사 중인지 여부
    private GameObject[] enemies; // 적 배열
    private GameObject currentTarget; // 현재 타겟 (적)

    void Start()
    {
        StartCoroutine(FindEnemies()); // 적을 찾는 코루틴 시작
    }

    void Update()
    {
        if (currentTarget != null)
        {
            float distance = Vector3.Distance(transform.position, currentTarget.transform.position);

            if (distance > 15f) // 거리가 15f을 넘으면 적을 향해 이동
            {
                ChaseTarget();
            }
            else if (distance <= 15f) // 거리가 15f 이내면 총알 발사
            {
                RotateTowardsTarget();
                if (!isFiring)
                {
                    StartCoroutine(FireAtTarget());
                }
            }
        }
    }

    // 적을 찾는 코루틴
    IEnumerator FindEnemies()
    {
        while (true)
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy"); // 적 찾기
            FindClosestTarget(); // 가장 가까운 적 찾기
            yield return new WaitForSeconds(1f); // 1초마다 적을 찾음
        }
    }

    // 가장 가까운 적 찾기
    void FindClosestTarget()
    {
        float closestDistance = Mathf.Infinity;
        currentTarget = null;

        if (enemies != null && enemies.Length > 0)
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distanceToEnemy < closestDistance)
                    {
                        closestDistance = distanceToEnemy;
                        currentTarget = enemy;
                    }
                }
            }
        }
    }

    // 적을 향해 회전
    void RotateTowardsTarget()
    {
        if (currentTarget != null)
        {
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            transform.up = -direction; // 적을 향해 회전
        }
    }

    // 적을 향해 이동
    void ChaseTarget()
    {
        if (currentTarget != null)
        {
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            transform.position += direction * 5f * Time.deltaTime; // 이동 속도 조절
            transform.up = -direction; // 적을 향해 회전
        }
    }

    // 총알 발사
    IEnumerator FireAtTarget()
    {
        isFiring = true;
        yield return new WaitForSeconds(0.5f); // 1초 대기 후 발사

        while (currentTarget != null && Vector3.Distance(transform.position, currentTarget.transform.position) <= 15f)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().SetDirection((currentTarget.transform.position - transform.position).normalized);
            bullet.GetComponent<Bullet>().from = gameObject.tag; // 아군이 발사한 총알임을 표시
            bullet.transform.rotation = transform.rotation;
            yield return new WaitForSeconds(fireRate);
        }
        isFiring = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 적의 총알에 맞았을 때
        if (collision.gameObject.CompareTag("Bullet") && collision.gameObject.GetComponent<Bullet>().from == "Enemy")
        {
            hp--; // 체력 감소
            collision.gameObject.SetActive(false); // 총알 비활성화

            if (damageParticle != null)
            {
                Instantiate(damageParticle, transform.position, Quaternion.identity); // 데미지 파티클 생성
            }

            // 체력이 0 이하이면 환자로 변환
            if (hp <= 0)
            {
                ConvertToPatient();
            }
        }
    }

    // 아군을 환자로 변환
    void ConvertToPatient()
    {
        if (patientPrefab != null)
        {
            Instantiate(patientPrefab, transform.position, Quaternion.identity); // 환자 생성
        }
        Destroy(gameObject); // 아군 오브젝트 제거
    }
}