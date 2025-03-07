using UnityEngine;

public class Ally : MonoBehaviour
{
    public int hp = 1; // �Ʊ��� ü��
    public GameObject patientPrefab; // ȯ�� ������
    public ParticleSystem damageParticle; // ������ ���� �� ��ƼŬ

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