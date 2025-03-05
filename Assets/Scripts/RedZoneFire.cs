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

    public GameObject redZoneBound;
    private bool isGrowing = true;



    float testInt = 0;

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


    // Update is called once per frame
    void Update()
    {
        if (isGrowing && testInt < 5)
        {
            testInt += Time.deltaTime * 5 / 3;
            gameObject.transform.localScale = new Vector3(testInt, testInt, testInt);
        }
        else
        {
            testInt = 5;
            isGrowing = false; // ũ�� ������ �Ϸ�Ǹ� �÷��׸� false�� ����
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
