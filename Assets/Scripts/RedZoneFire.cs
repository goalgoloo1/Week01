using System.Collections;
using UnityEngine;

public class RedZoneFire : MonoBehaviour
{
    public float appearTime = 3f; // �������� ������ Ȱ��ȭ�� �ð�
    public float dangerTime = 1f; // Ȱ��ȭ�� �� �����Ǵ� �ð�
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    public ParticleSystem explosionParticle;
    private float deleteExplosion = 5f;
    private FollowPlayer cameraShake;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        cameraShake = Camera.main.GetComponent<FollowPlayer>();

        StartCoroutine(ActivateRedZone());
    }

    IEnumerator ActivateRedZone()
    {
        // �ʱ� ����: ������ & �浹 ��Ȱ��ȭ
        Color color = spriteRenderer.color;
        color.a = 0.3f; // ������ ����
        spriteRenderer.color = color;
        col.enabled = false; // �浹 ��Ȱ��ȭ

        // appearTime ���� ��� (������ ���� ����)
        yield return new WaitForSeconds(appearTime);

        // ������ Ȱ��ȭ (������ ���̰� ����� �浹 Ȱ��ȭ)
        color.a = 1f; // �������ϰ� ����
        spriteRenderer.color = color;
        col.enabled = true; // �浹 Ȱ��ȭ
        TriggerExplosion();

        if (cameraShake != null)
        {
            StartCoroutine(cameraShake.CameraShake(0.5f, 0.3f)); // 0.5�� ���� ���� 0.3���� ��鸲
        }

        // dangerTime �Ŀ� ������ ��Ȱ��ȭ
        yield return new WaitForSeconds(dangerTime);
        Destroy(gameObject); // ������ ����
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void TriggerExplosion()
    {
        if (explosionParticle != null)
        {
            // ���� ��ƼŬ ����
            ParticleSystem explosion = Instantiate(explosionParticle, transform.position, Quaternion.identity);
            explosion.Play();
            Destroy(explosion.gameObject, deleteExplosion);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }

        if(collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
