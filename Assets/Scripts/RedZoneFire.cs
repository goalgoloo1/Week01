using System.Collections;
using UnityEngine;

public class RedZoneFire : MonoBehaviour
{
    public float appearTime = 3f; // 레드존이 완전히 활성화될 시간
    public float dangerTime = 1f; // 활성화된 후 유지되는 시간
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
        // 초기 상태: 반투명 & 충돌 비활성화
        Color color = spriteRenderer.color;
        color.a = 0.3f; // 반투명 설정
        spriteRenderer.color = color;
        col.enabled = false; // 충돌 비활성화

        // appearTime 동안 대기 (반투명 상태 유지)
        yield return new WaitForSeconds(appearTime);

        // 레드존 활성화 (완전히 보이게 만들고 충돌 활성화)
        color.a = 1f; // 불투명하게 변경
        spriteRenderer.color = color;
        col.enabled = true; // 충돌 활성화
        TriggerExplosion();

        if (cameraShake != null)
        {
            StartCoroutine(cameraShake.CameraShake(0.5f, 0.3f)); // 0.5초 동안 강도 0.3으로 흔들림
        }

        // dangerTime 후에 레드존 비활성화
        yield return new WaitForSeconds(dangerTime);
        Destroy(gameObject); // 레드존 제거
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void TriggerExplosion()
    {
        if (explosionParticle != null)
        {
            // 폭발 파티클 실행
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
