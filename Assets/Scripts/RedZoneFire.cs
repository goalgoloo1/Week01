using System.Collections;
using UnityEngine;

public class RedZoneFire : MonoBehaviour
{
    public float appearTime = 3f; // �������� ������ Ȱ��ȭ�� �ð�
    public float dangerTime = 0.1f; // Ȱ��ȭ�� �� �����Ǵ� �ð�
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    public ParticleSystem explosionParticle;
    private float deleteExplosion = 5f;
    private FollowPlayer cameraShake;

    public GameObject redZoneBound;
    private bool isGrowing = true;

    public bool isboom = false;

    float testInt = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        cameraShake = Camera.main.GetComponent<FollowPlayer>();

        StartCoroutine(ActivateRedZone());

        redZoneBound.transform.localScale = new Vector3(size,size,1);
    }

    IEnumerator ActivateRedZone()
    {
        // �ʱ� ����: ������ & �浹 ��Ȱ��ȭ
        Color color = spriteRenderer.color;
        color.a = 0.3f;
        spriteRenderer.color = color;
        col.enabled = false;

        yield return new WaitForSeconds(appearTime);

        // ������ Ȱ��ȭ
        color.a = 1f;
        spriteRenderer.color = color;
        col.enabled = true;
        TriggerExplosion();

        if (cameraShake != null)
        {
            StartCoroutine(cameraShake.CameraShake(0.5f, 0.3f));
        }

        // ũ�� ������ �Ϸ�� ������ ���
        while (isGrowing)
        {
            yield return null;
        }

        // dangerTime �Ŀ� ������ ��Ȱ��ȭ
        yield return new WaitForSeconds(dangerTime);
        DestroyObjects();
        Destroy(gameObject);
    }

    private void DestroyObjects()
    {
        if (redZoneBound != null)
        {
            Destroy(redZoneBound);
        }
    }


    int size = 10;
    // Update is called once per frame
    void Update()
    {
        if (isGrowing && testInt < size)
        {
            testInt += Time.deltaTime * size / 3;
            gameObject.transform.localScale = new Vector3(testInt, testInt, testInt);
        }
        else
        {
            testInt = size;
            isGrowing = false; // ũ�� ������ �Ϸ�Ǹ� �÷��׸� false�� ����
        }

        if (isboom)
        {
            Color c = GetComponent<SpriteRenderer>().color;
            c.a -= 0.5f * Time.deltaTime;
            GetComponent<SpriteRenderer>().color = c;
        }
        else 
        {
            Color c = GetComponent<SpriteRenderer>().color;
            c.a += 0.2f * Time.deltaTime;
            GetComponent<SpriteRenderer>().color = c;
        }
    }

    void TriggerExplosion()
    {
        if (explosionParticle != null)
        {
            // ���� ��ƼŬ ����
            ParticleSystem explosion = Instantiate(explosionParticle, transform.position, Quaternion.identity);
            explosion.Play();
            Destroy(explosion.gameObject, deleteExplosion);
            isboom = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isGrowing && collision.CompareTag("Player"))
        {
            DestroyObjects();
            Destroy(gameObject);
            Destroy(collision.gameObject);
            GameManager gm = GameObject.FindFirstObjectByType<GameManager>();
            gm.Gameover();
        }

        if (!isGrowing && collision.CompareTag("Enemy"))
        {
            DestroyObjects();
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
