using UnityEngine;
using UnityEngine.AI;
using CodeMonkey.Utils;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    // 플레이어의 Transform
    public Transform player;
    private Vector3 lastPlayerPosition; // 마지막으로 본 플레이어 위치

    // 뒤에서 암살
    [SerializeField] GameObject PressE_UI;
    public float assassinRange = 1f; // 공격 범위
    public float rotationThresholdMin = 130f;
    public float rotationThresholdMax = 180f;

    // 실제 raycast 시야각
    [SerializeField] private LayerMask layerMask;
    public float wideFov;
    public float wideViewDistance;
    public float longFov;
    public float longViewDistance;

    // 시야각 UI 가져오기
    [SerializeField] private FieldOfViewEnemyWide_Script fieldOfViewEnemyWide;
    [SerializeField] private FieldOfViewEnemyLong_Script fieldOfViewEnemyLong;
    [SerializeField] private FoVEnemyColor_Script fieldOfViewEnemyWideColor;
    [SerializeField] private FoVEnemyColor_Script fieldOfViewEnemyLongColor;
    public GameObject fieldOfViewEnemyPrefabWide;
    public GameObject fieldOfViewEnemyPrefabLong;

    // 인공지능
    public NavMeshAgent agent;
    public enum EnemyState
    {
        Patrolling,
        Chasing,
        Searching
    }
    public EnemyState currentState = EnemyState.Patrolling;

    // ZoneMove
    public List<Vector3> zoneMovePoints;
    private int currentZoneMovePointIndex;
    public bool isWalkingToZoneMovePoint;
    // Patroling
    public Vector3 walkPoint;
    bool isWalkPointSet;
    // Attacking
    public float timeBetweenAttacks;
    bool isAlreadyAttacked;
    // Searching
    public float searchingWalkPointRange;
    public float searchingTime;








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

        CheckFieldOfView(wideFov, wideViewDistance);
        CheckFieldOfView(longFov, longViewDistance);


        // AI 3가지 상황 체크
        switch (currentState)
        {
            case EnemyState.Patrolling:
                ZoneMove();
                fieldOfViewEnemyWideColor.ChangeFoVColor(fieldOfViewEnemyWideColor.white);
                fieldOfViewEnemyLongColor.ChangeFoVColor(fieldOfViewEnemyLongColor.white);
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                fieldOfViewEnemyWideColor.ChangeFoVColor(fieldOfViewEnemyWideColor.red);
                fieldOfViewEnemyLongColor.ChangeFoVColor(fieldOfViewEnemyLongColor.red);
                break;
            case EnemyState.Searching:
                Searching();
                fieldOfViewEnemyWideColor.ChangeFoVColor(fieldOfViewEnemyWideColor.yellow);
                fieldOfViewEnemyLongColor.ChangeFoVColor(fieldOfViewEnemyLongColor.yellow);
                break;
        }

        // if (!isWalkingToZoneMovePoint) ZoneMove();
        // if (!isPlayerInSightRange && !isPlayerInAttackRange) Patroling();
        // if (isPlayerInSightRange && !isPlayerInAttackRange) ChasePlayer();
        // if (isPlayerInSightRange && isPlayerInAttackRange) AttackPlayer();

        // 암살시 적과 플레이어 거리 확인
        Vector3 enemyPosition = transform.position;
        Vector3 directionToPlayer = player.position - enemyPosition;
        float distance = directionToPlayer.magnitude;

        // 암살시 플레이어가 공격 범위 내에 있는지 확인
        if (distance < assassinRange)
        {
            float angleToPlayer = Vector3.SignedAngle(transform.up, directionToPlayer.normalized, Vector3.forward);
            float angleDifference = Mathf.Abs(angleToPlayer);

            // 각도 차이가 지정한 범위에 있는지 확인
            if (angleDifference >= rotationThresholdMin && angleDifference <= rotationThresholdMax)
            {
                ShowEKeyUI(true);

                // E 키를 눌렀는지 확인
                if (Input.GetKeyDown(KeyCode.E))
                {
                    KillEnemy();
                }
            }
        }
        else
        {
            ShowEKeyUI(false);
        }
    }

    public void FOVStart()
    {
        // Field of View Wide 오브젝트 생성
        GameObject fovObjectWide = Instantiate(fieldOfViewEnemyPrefabWide, Vector2.zero, Quaternion.identity);
        fieldOfViewEnemyWide = fovObjectWide.GetComponent<FieldOfViewEnemyWide_Script>();
        fieldOfViewEnemyWideColor = fovObjectWide.GetComponent<FoVEnemyColor_Script>();
        fieldOfViewEnemyWide.enemy = this; // Field of View에 자신을 참조하게 설정
        fieldOfViewEnemyWideColor.enemy = this;

        // Field of View Long 오브젝트 생성
        GameObject fovObjectLong = Instantiate(fieldOfViewEnemyPrefabLong, Vector2.zero, Quaternion.identity);
        fieldOfViewEnemyLong = fovObjectLong.GetComponent<FieldOfViewEnemyLong_Script>();
        fieldOfViewEnemyLongColor = fovObjectLong.GetComponent<FoVEnemyColor_Script>();
        fieldOfViewEnemyLong.enemy = this; // Field of View에 자신을 참조하게 설정
        fieldOfViewEnemyLongColor.enemy = this;

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
        float currentAngle = transform.eulerAngles.z;

        // 시야각의 시작과 끝 각도 계산
        float wideFOVStartAngle = currentAngle - wideFov / 2; // 넓은 시야각 시작
        float wideFOVEndAngle = currentAngle + wideFov / 2;   // 넓은 시야각 끝
        float longFOVStartAngle = currentAngle - longFov / 2;  // 좁은 시야각 시작
        float longFOVEndAngle = currentAngle + longFov / 2;    // 좁은 시야각 끝

        // Wide FOV 설정
        fieldOfViewEnemyWide.SetAimDirection(UtilsClass.GetVectorFromAngle(wideFOVStartAngle + 90 + wideFov * 2 - wideFov / 2)); // 각도 보정 추가
        fieldOfViewEnemyWide.SetOrigin(this.transform.position);

        // Long FOV 설정
        fieldOfViewEnemyLong.SetAimDirection(UtilsClass.GetVectorFromAngle(longFOVStartAngle + 90 + longFov * 2 - longFov / 2)); // 각도 보정 추가
        fieldOfViewEnemyLong.SetOrigin(this.transform.position);
    }

    private void CheckFieldOfView(float _fov, float _viewDistance)
    {
        Vector3 origin = transform.position;
        float startingAngle = transform.eulerAngles.z - _fov / 2f + 90;

        int rayCount = 50;
        float angleIncrease = _fov / rayCount;

        for (int i = 0; i <= rayCount; i++)
        {
            float angle = startingAngle + angleIncrease * i;
            RaycastHit2D hit = Physics2D.Raycast(origin, UtilsClass.GetVectorFromAngle(angle), _viewDistance, layerMask);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("플레이어를 발견했습니다!");
                    lastPlayerPosition = hit.collider.transform.position; // 마지막으로 본 플레이어 위치 저장
                    currentState = EnemyState.Chasing; // 상태를 Chasing으로 변경
                }
                else
                {
                    Debug.Log("플레이어가 아닌 오브젝트에 닿았습니다." + hit.collider.name);
                }
            }
            else
            {
                Debug.Log("어떤 오브젝트에도 닿지 않았습니다.");
            }

            // Gizmos를 통해 Ray 시각화
            Debug.DrawRay(origin, UtilsClass.GetVectorFromAngle(angle) * _viewDistance, Color.red);
        }
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

    void ShowEKeyUI(bool _isPlayerClose)
    {
        if (_isPlayerClose == true)
        {
            PressE_UI.SetActive(true);
        }
        else
        {
            PressE_UI.SetActive(false);
        }
    }

    void KillEnemy()
    {
        gameObject.SetActive(false);
    }

    private void ChasePlayer()
    {
        agent.SetDestination(lastPlayerPosition); // 마지막으로 본 위치로 이동

        // 마지막 위치에 도착했는지 확인
        if (Vector3.Distance(transform.position, lastPlayerPosition) < 1f)
        {
            currentState = EnemyState.Searching;
            Invoke(nameof(ReturnToPatrolling), searchingTime); // searchingTime 대기 후 패트롤 상태로 돌아가기
        }
    }

    private void ReturnToPatrolling()
    {
        currentState = EnemyState.Patrolling; // 다시 패트롤 상태로 변경
    }


    void Searching()
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
        float randomX = Random.Range(-searchingWalkPointRange, searchingWalkPointRange);
        float randomY = Random.Range(-searchingWalkPointRange, searchingWalkPointRange);

        Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y + randomY, 0);

        NavMeshHit hit;

        // NavMesh에서 walkable한 위치를 찾기
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            walkPoint = hit.position; // 찾은 위치를 walkPoint로 설정
            isWalkPointSet = true;
        }
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

}
