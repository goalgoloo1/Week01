using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f; // 이동 속도
    private Transform player; // 플레이어 위치 저장
    [SerializeField]private float lifeTime = 5f; // 일정 시간이 지나면 사라짐

    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Invoke(nameof(DeactivateEnemy), lifeTime); // 일정 시간이 지나면 오브젝트 풀로 반환
    }

    void Update()
    {
        if (player != null)
        {
            // 플레이어를 향해 이동
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    void DeactivateEnemy()
    {
        ObjectPooling.ReturnEnemy(gameObject);
    }

    void OnDisable()
    {
        CancelInvoke(); // 비활성화될 때 Invoke 취소
    }
}

