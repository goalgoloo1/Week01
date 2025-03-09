using UnityEngine;
using UnityEngine.AI;
using CodeMonkey.Utils;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    // 플레이어의 Transform
    public Transform player; 

    // 시야각 가져오기
    [SerializeField] private FieldOfViewEnemyWide_Script fieldOfViewEnemyWide;
    [SerializeField] private FieldOfViewEnemyLong_Script fieldOfViewEnemyLong;
    public GameObject fieldOfViewEnemyPrefabWide;
    public GameObject fieldOfViewEnemyPrefabLong;

    // 인공지능
    public NavMeshAgent agent;

    // ZoneMove
    public List<Vector3> zoneMovePoints;
    private int currentZoneMovePointIndex;
    public bool isWalkingToZoneMovePoint;
    // Patroling
    public Vector3 walkPoint;
    public bool isWalkPointSet;
    public float walkPointRange;
    // Attacking
    public float timeBetweenAttacks;
    bool isAlreadyAttacked;
    // States
    public float sightRange, attackRange, sightAngle, attackAngle;
    public bool isPlayerInSightRange, isPlayerInAttackRange;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        // 처음 인덱스를 0으로 설정, 처음에는 이동하지 않음
        currentZoneMovePointIndex = 0;
        isWalkingToZoneMovePoint = false;

        FOVStart();
    }

    void Update()
    {
        FOVUpdate();

        // 시야와 공격 범위 체크
        isPlayerInSightRange = Physics.CheckSphere(transform.position, sightRange);
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange);

        // AI 3가지 상황 체크
        if (!isWalkingToZoneMovePoint) ZoneMove();
        // if (!isPlayerInSightRange && !isPlayerInAttackRange) Patroling();
        if (isPlayerInSightRange && !isPlayerInAttackRange) ChasePlayer();
        if (isPlayerInSightRange && isPlayerInAttackRange) AttackPlayer();
    }

    public void FOVStart()
    {
        // Field of View Wide 오브젝트 생성
        GameObject fovObjectWide = Instantiate(fieldOfViewEnemyPrefabWide, Vector2.zero, Quaternion.identity);
        fieldOfViewEnemyWide = fovObjectWide.GetComponent<FieldOfViewEnemyWide_Script>();
        fieldOfViewEnemyWide.enemy = this; // Field of View에 자신을 참조하게 설정

        // Field of View Long 오브젝트 생성
        GameObject fovObjectLong = Instantiate(fieldOfViewEnemyPrefabLong, Vector2.zero, Quaternion.identity);
        fieldOfViewEnemyLong = fovObjectLong.GetComponent<FieldOfViewEnemyLong_Script>();
        fieldOfViewEnemyLong.enemy = this; // Field of View에 자신을 참조하게 설정

        // 시야각의 시작 위치와 회전 설정
        fieldOfViewEnemyWide.SetOrigin(transform.position);
        fieldOfViewEnemyWide.transform.rotation = transform.rotation; // 적과 같은 회전으로 설정

        // 시야각의 시작 위치와 회전 설정
        fieldOfViewEnemyLong.SetOrigin(transform.position);
        fieldOfViewEnemyLong.transform.rotation = transform.rotation; // 적과 같은 회전으로 설정
    }

    public void FOVUpdate()
    {
        // 적의 현재 각도를 가져와 시야각을 업데이트
        float currentAngle = transform.eulerAngles.z; // 왜 235, 120을 더해야 하는지는 모르겠음...ㅠㅠㅠ
        fieldOfViewEnemyWide.SetAimDirection(UtilsClass.GetVectorFromAngle(currentAngle + 235));
        fieldOfViewEnemyWide.SetOrigin(this.transform.position);
        fieldOfViewEnemyLong.SetAimDirection(UtilsClass.GetVectorFromAngle(currentAngle + 120));
        fieldOfViewEnemyLong.SetOrigin(this.transform.position);
    }

    void ZoneMove()
    {
        if (zoneMovePoints.Count == 0)
            return; // 탐사할 포인트가 없으면 종료

        // 현재 목표 포인트 설정
        Vector3 walkPoint = zoneMovePoints[currentZoneMovePointIndex];
        agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // walkPoint 도달 시
        if (distanceToWalkPoint.magnitude < 1f)
        {
            // 다음 포인트로 이동
            currentZoneMovePointIndex++;
            if (currentZoneMovePointIndex >= zoneMovePoints.Count)
            {
                currentZoneMovePointIndex = 0; // 마지막 포인트에 도달하면 처음으로 돌아감
            }
        }
    }


    void Patroling()
    {
        if (!isWalkPointSet)
        {
            SearchWalkPoint();
        }
        else if (isWalkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // walkPoint 도달 시
        if (distanceToWalkPoint.magnitude < 1f)
            isWalkPointSet = false;
    }

    void SearchWalkPoint()
    {
        // 범위 내 랜덤한 포인트 지정
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomY = Random.Range(-walkPointRange, walkPointRange);

        Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y + randomY, 0);

        NavMeshHit hit;

        // NavMesh에서 walkable한 위치를 찾기
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            walkPoint = hit.position; // 찾은 위치를 walkPoint로 설정
            isWalkPointSet = true;
        }
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        // 적이 움직이지 않도록 하는 코드
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!isAlreadyAttacked)
        {
            // 어택 코드
            // 어택 코드
            // 어택 코드


            isAlreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        isAlreadyAttacked = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, walkPointRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);  
    }

}
