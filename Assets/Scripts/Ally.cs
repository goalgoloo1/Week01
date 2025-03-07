using UnityEngine;

public class Ally : MonoBehaviour
{
    public int hp = 1; // 아군의 체력
    public GameObject patientPrefab; // 환자 프리팹
    public ParticleSystem damageParticle; // 데미지 입을 때 파티클

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