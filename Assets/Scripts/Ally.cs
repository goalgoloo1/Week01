using UnityEngine;
using System.Collections;

public class Ally : MonoBehaviour
{
    public int hp = 5; // �Ʊ��� ü��
    public GameObject patientPrefab; // ȯ�� ������
    public ParticleSystem damageParticle; // ������ ���� �� ��ƼŬ
    public GameObject bulletPrefab; // �Ѿ� ������
    public Transform firePoint; // �Ѿ� �߻� ��ġ
    private float fireRate = 2f; // �߻� �ֱ�
    private bool isFiring = false; // �߻� ������ ����
    private GameObject[] enemies; // �� �迭
    private GameObject currentTarget; // ���� Ÿ�� (��)

    void Start()
    {
        StartCoroutine(FindEnemies()); // ���� ã�� �ڷ�ƾ ����
    }

    void Update()
    {
        if (currentTarget != null)
        {
            float distance = Vector3.Distance(transform.position, currentTarget.transform.position);

            if (distance > 15f) // �Ÿ��� 15f�� ������ ���� ���� �̵�
            {
                ChaseTarget();
            }
            else if (distance <= 15f) // �Ÿ��� 15f �̳��� �Ѿ� �߻�
            {
                RotateTowardsTarget();
                if (!isFiring)
                {
                    StartCoroutine(FireAtTarget());
                }
            }
        }
    }

    // ���� ã�� �ڷ�ƾ
    IEnumerator FindEnemies()
    {
        while (true)
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy"); // �� ã��
            FindClosestTarget(); // ���� ����� �� ã��
            yield return new WaitForSeconds(1f); // 1�ʸ��� ���� ã��
        }
    }

    // ���� ����� �� ã��
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

    // ���� ���� ȸ��
    void RotateTowardsTarget()
    {
        if (currentTarget != null)
        {
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            transform.up = -direction; // ���� ���� ȸ��
        }
    }

    // ���� ���� �̵�
    void ChaseTarget()
    {
        if (currentTarget != null)
        {
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            transform.position += direction * 5f * Time.deltaTime; // �̵� �ӵ� ����
            transform.up = -direction; // ���� ���� ȸ��
        }
    }

    // �Ѿ� �߻�
    IEnumerator FireAtTarget()
    {
        isFiring = true;
        yield return new WaitForSeconds(0.5f); // 1�� ��� �� �߻�

        while (currentTarget != null && Vector3.Distance(transform.position, currentTarget.transform.position) <= 15f)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().SetDirection((currentTarget.transform.position - transform.position).normalized);
            bullet.GetComponent<Bullet>().from = gameObject.tag; // �Ʊ��� �߻��� �Ѿ����� ǥ��
            bullet.transform.rotation = transform.rotation;
            yield return new WaitForSeconds(fireRate);
        }
        isFiring = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���� �Ѿ˿� �¾��� ��
        if (collision.gameObject.CompareTag("Bullet") && collision.gameObject.GetComponent<Bullet>().from == "Enemy")
        {
            hp--; // ü�� ����
            collision.gameObject.SetActive(false); // �Ѿ� ��Ȱ��ȭ

            if (damageParticle != null)
            {
                Instantiate(damageParticle, transform.position, Quaternion.identity); // ������ ��ƼŬ ����
            }

            // ü���� 0 �����̸� ȯ�ڷ� ��ȯ
            if (hp <= 0)
            {
                ConvertToPatient();
            }
        }
    }

    // �Ʊ��� ȯ�ڷ� ��ȯ
    void ConvertToPatient()
    {
        if (patientPrefab != null)
        {
            Instantiate(patientPrefab, transform.position, Quaternion.identity); // ȯ�� ����
        }
        Destroy(gameObject); // �Ʊ� ������Ʈ ����
    }
}