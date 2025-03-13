using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy_Soldier : CharacterBase
{
	Rigidbody2D rb;
    public GameObject player;
    enum DistanceAI
    {
        none = 0,
        far = 1 << 1,
        find = 1 << 2,
        canShot = 1 << 3,
    }
    DistanceAI currentAI = DistanceAI.none;
    private void OnEnable()
    {
		FindWeapon();
    }
    void Start()
	{
		hp = 5;
		moveSpeed = 10f;
		deadParticle = Resources.Load<GameObject>("Particles/Damage_Particle");
		Player p = GameObject.FindFirstObjectByType<Player>();
		if (p) player = p.gameObject;
		rb = GetComponent<Rigidbody2D>();
	}
	void Update()
	{
        // 체력이 0이면 충돌없애고 멈추면 사망
        if (hp <= 0) 
		{
			GetComponent<CapsuleCollider2D>().enabled = false;
			stunTime += 99;
            StartCoroutine(DeadTime());
        }
        IEnumerator DeadTime() 
		{
			yield return new WaitForSeconds(0.5f);
            SpawnManager sm = GameObject.FindFirstObjectByType<SpawnManager>();
			sm.score++;
            Dead();
		}

		// 스턴시간감소
		if (stunTime > 0)
		{
			stunTime -= Time.deltaTime;
			transform.Find("Usual").gameObject.SetActive(false);
			transform.Find("TakeDamage").gameObject.SetActive(true);
		}
		else if(stunTime < 0)
		{
			stunTime = 0;
            transform.Find("Usual").gameObject.SetActive(true);
            transform.Find("TakeDamage").gameObject.SetActive(false);
        }
		if (!player || stunTime > 0)
		{
			CastFOVRays(transform.position, -transform.up, 90f, 15f);
			wait = 0;
		}
		else 
		{
            Vector2 arrowDirection = (player.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + 90);

            float playerDistance = Vector2.Distance(player.transform.position, transform.position);
			if (playerDistance > 20f)
			{
            }
			else if (playerDistance <= 20f && playerDistance > 10f)
			{
				if (stunTime == 0)
				{
					Vector3 toPlayerDirection = (player.transform.position - transform.position).normalized;
					rb.linearVelocity = toPlayerDirection * moveSpeed;
				}
			}
			else if(playerDistance <= 10f) 
			{
				if (wait > waitTime)
				{
					weapon.GetComponent<Weapon>().Fire();
					wait = 0;
				}
				else 
				{
					wait += Time.deltaTime;
				}
			}
        }
    }
    float waitTime = 1.5f;
    float wait = 0;
    public void CastFOVRays(Vector2 origin, Vector2 direction, float fov, float range)
    {
		Debug.Log("Cast함수");
        float halfFOV = fov / 2; // 시야각 절반
        float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 기준 방향의 각도

   //     for (float angleOffset = -halfFOV; angleOffset <= halfFOV; angleOffset += 2f) // 2도 간격 발사
   //     {
   //         float rayAngle = baseAngle + angleOffset; // 최종 방향

   //         Vector2 rayDir = new Vector2(Mathf.Cos(rayAngle * Mathf.Deg2Rad), Mathf.Sin(rayAngle * Mathf.Deg2Rad)); // 각도->벡터 변환
   //         RaycastHit2D hit = Physics2D.Raycast(origin, rayDir, range, 1 << 3);
   //         Debug.DrawRay(origin, rayDir * range, hit.collider ? Color.red : Color.green, 0.1f);
			//if (hit)
			//{
			//	Debug.Log("플레이어 찾음");
			//	player = hit.collider.gameObject;
			//	break;
			//}
			//else 
			//{
   //             Debug.Log("플레이어 못찾음");
   //         }
   //     }
    }
}
