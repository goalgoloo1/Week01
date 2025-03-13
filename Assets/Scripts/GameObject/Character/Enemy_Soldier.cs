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
        // ü���� 0�̸� �浹���ְ� ���߸� ���
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

		// ���Ͻð�����
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
		Debug.Log("Cast�Լ�");
        float halfFOV = fov / 2; // �þ߰� ����
        float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // ���� ������ ����

   //     for (float angleOffset = -halfFOV; angleOffset <= halfFOV; angleOffset += 2f) // 2�� ���� �߻�
   //     {
   //         float rayAngle = baseAngle + angleOffset; // ���� ����

   //         Vector2 rayDir = new Vector2(Mathf.Cos(rayAngle * Mathf.Deg2Rad), Mathf.Sin(rayAngle * Mathf.Deg2Rad)); // ����->���� ��ȯ
   //         RaycastHit2D hit = Physics2D.Raycast(origin, rayDir, range, 1 << 3);
   //         Debug.DrawRay(origin, rayDir * range, hit.collider ? Color.red : Color.green, 0.1f);
			//if (hit)
			//{
			//	Debug.Log("�÷��̾� ã��");
			//	player = hit.collider.gameObject;
			//	break;
			//}
			//else 
			//{
   //             Debug.Log("�÷��̾� ��ã��");
   //         }
   //     }
    }
}
