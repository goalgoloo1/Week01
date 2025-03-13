using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class Player : CharacterBase
{
    public enum PlayerState
    {
        idle = 0,
        move = 1 << 1,
        runKeyHolding = 1 << 2,
        running = move | runKeyHolding,
        doing = 1 << 3,
        selfhealing = 1 << 4,
        meleeAttack = 1 << 5,
        fire = 1 << 6,
        rolling = 1 << 7
    }
    public PlayerState currentState = PlayerState.idle;

    public Rigidbody2D rb;
	public InputActionAsset actionAsset;
	public InputActionMap actionMap;
	public Vector2 moveDir;
	public Dictionary<string, InputAction> inputActions;
    public Vector2 rollDir = Vector2.zero;
	public float meleeAttackCharge = 0;
    private void OnEnable()
	{
        inputActions = new Dictionary<string, InputAction>();
        if (actionAsset.FindActionMap("PlayerActions") != null)
        {
            actionAsset.FindActionMap("PlayerActions").Enable();
        }
        else 
        {
            Debug.Log("playerActions찾을 수 없음");
        }
            actionMap = actionAsset.FindActionMap("PlayerActions");
		foreach (InputAction IA in actionMap.actions) 
		{
			if (IA != null)
			{
				inputActions.Add(IA.name, IA);
				Debug.Log(IA.name);
			}
		}
        rb = GetComponent<Rigidbody2D>();
        FindWeapon();
        somethings = new List<GameObject>();
    }
    private void OnDisable()
	{
		actionAsset.FindActionMap("PlayerActions").Disable();
	}

	public float movemultiply = 1;
	private void Start()
	{
		hp = 99;
		moveSpeed = 15;
		maxStamina = 1.5f;
		curStamina = maxStamina;
		beforeStamina = curStamina;
        fastMoveMultiply = 1.5f;
        deadParticle = Resources.Load<GameObject>("Particles/Damage_Particle");

		// 키등록
        inputActions["Move"].started += _ => addState(PlayerState.move);
        inputActions["Move"].canceled += _ => removeState(PlayerState.move);
        inputActions["Run"].started += _ => addState(PlayerState.runKeyHolding);
        inputActions["Run"].canceled += _ => removeState(PlayerState.runKeyHolding);
        inputActions["Save"].started += _ => addState(PlayerState.doing);
        inputActions["Save"].canceled += _ => removeState(PlayerState.doing);
        inputActions["Heal"].started += _ => addState(PlayerState.selfhealing);
        inputActions["Heal"].canceled += _ => removeState(PlayerState.selfhealing);
        inputActions["MeleeAttack"].started += _ => MeleeAttackChargeStart();
        inputActions["MeleeAttack"].canceled += _ => MeleeAttackChargeAttack();
        inputActions["Fire"].started += _ => addState(PlayerState.fire);
        inputActions["Fire"].canceled += _ => removeState(PlayerState.fire);
        inputActions["Rolling"].started += _ => addState(PlayerState.rolling);
		//inputActions["Rolling"].canceled += _ => removeState(PlayerState.rolling);
    }

	float angle;
    private void Update()
    {
        if (HitStop.waiting) return;
        // 플레이어 각도
        if (!HasState(PlayerState.rolling) && !HasState(PlayerState.meleeAttack))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector2 direction = (mousePosition - transform.position).normalized;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
        if (!HasState(PlayerState.rolling))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
        }

        if (hp == 0)
        {
            Dead();
        }

        if (currentState == PlayerState.doing || currentState == PlayerState.selfhealing)
        {
            doingGauge += Time.deltaTime;
        }
        else 
        {
            doingGauge = 0;
        }
    }

	private void FixedUpdate()
	{
		// 가만히 있으면
		if(currentState == PlayerState.idle)
		{
            curStamina += Time.deltaTime*2;
        }
		if (currentState == PlayerState.move) 
		{
            curStamina += Time.deltaTime;
        }

        // 움직임 조건
        if (HasState(PlayerState.move))
		{
            moveDir = inputActions["Move"].ReadValue<Vector2>();
		}
		else 
		{
			moveDir = Vector2.zero;
		}
		// 달리기 조건
		if (HasState(PlayerState.move) && HasState(PlayerState.runKeyHolding) && curStamina > 0 && !HasState(PlayerState.rolling))
		{
			fastMoveMultiply = 2;
			curStamina -= Time.deltaTime;
		}
		else if(!HasState(PlayerState.rolling))
		{
			fastMoveMultiply = 1;
		}
		// doing 조건
		if (currentState == PlayerState.doing)
		{
		}
		// 자힐조건
		if (currentState == PlayerState.selfhealing)
		{
		}
		// 격발가능조건
		if (HasState(PlayerState.fire) && weaponScript.fireRate <= weaponScript.rateGauge && weaponScript.remainBullet > 0 && !HasState(PlayerState.rolling))
		{
			weaponScript.Fire();
		}
		//구르고 있으면 방향 못바구게
        if (HasState(PlayerState.rolling)) 
		{
            moveDir = rollDir;
		}
        // 근접공격 차징
        if (HasState(PlayerState.meleeAttack))
        {
            meleeAttackCharge += Time.deltaTime * 3;
            meleeAttackCharge = Mathf.Clamp(meleeAttackCharge, 0, 2f);
            transform.Find("LeftHand").localRotation = Quaternion.Euler(0, 0, meleeAttackCharge * 29);
            fastMoveMultiply = 0.5f;
        }

        curStamina = Mathf.Clamp(curStamina, 0, maxStamina);
        rb.linearVelocity = moveDir * moveSpeed * fastMoveMultiply;

        // UI에 정보 전달
        MainCanvas mainCanvas = GameObject.FindFirstObjectByType<MainCanvas>();
        mainCanvas.GetComponent<MainCanvas>().ShowStamina(maxStamina, curStamina);

    }

    // 근접공격차징조건
    void MeleeAttackChargeStart()
    {
        if (!HasState(PlayerState.rolling) && !HasState(PlayerState.meleeAttack))
        {
			Debug.Log("차징시작");
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector2 direction = (mousePosition - transform.position).normalized;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            addState(PlayerState.meleeAttack);
			transform.Find("LeftHand").localRotation = Quaternion.Euler(0,0,0);
            transform.Find("LeftHand").gameObject.SetActive(true);
			//transform.Find("LeftHand").GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
        }
    }
    // 근접공격 발사
    void MeleeAttackChargeAttack() 
	{
        Debug.Log("근접공격");
        transform.Find("LeftHand").GetChild(0).GetComponent<LeftFist>().ispunching = true;
        StartCoroutine(Punch());
    }

	IEnumerator Punch() 
	{
        if (meleeAttackCharge > 0.05f)
        {
            //transform.Find("LeftHand").GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
            transform.Find("LeftHand").GetChild(0).GetComponent<LeftFist>().power = meleeAttackCharge;
            float targetAngle = -120f;  // 목표 각도
            float rotationTime = 0.1f;  // 회전 시간
            float elapsedTime = 0f;
            float originRotation = transform.Find("LeftHand").localEulerAngles.z;
            float AngleDistance = Mathf.Abs(targetAngle - originRotation);

            // 회전이 진행 중일 때
            while (elapsedTime < rotationTime)
            {
                elapsedTime += Time.deltaTime;  // 경과 시간 갱신
                float t = elapsedTime / rotationTime;  // 진행도

                // 현재 회전 값
                Vector3 curAngle = transform.Find("LeftHand").localEulerAngles;
                // 목표 각도로 부드럽게 회전
                curAngle.z = originRotation - AngleDistance * t;
                // 회전 적용
                transform.Find("LeftHand").localRotation = Quaternion.Euler(curAngle);
                yield return 0;
            }
        }
        //펀치 끝
        meleeAttackCharge = 0;
        transform.Find("LeftHand").GetChild(0).GetComponent<LeftFist>().ispunching = false;
        transform.Find("LeftHand").GetChild(0).GetComponent<LeftFist>().list.Clear();
        removeState(PlayerState.meleeAttack);
        transform.Find("LeftHand").gameObject.SetActive(false);
    }

    // 구르기
    IEnumerator RollAnimation()
    {
        curStamina -= 0.5f;
        for (int i = 1; i <= 5; i++)
        {
            if (i == 1)
            {
                transform.Find("Idle").gameObject.SetActive(false);
                transform.Find("Rolling1").gameObject.SetActive(true);
				fastMoveMultiply = 4f;
                yield return new WaitForSeconds(0.05f);
            }
            else
            {
                transform.Find("Rolling" + (i - 1).ToString()).gameObject.SetActive(false);
                transform.Find("Rolling" + i.ToString()).gameObject.SetActive(true);
            }
            fastMoveMultiply /= 1.5f;
			fastMoveMultiply = Mathf.Clamp(fastMoveMultiply, 0, 100);
            yield return new WaitForSeconds(0.08f);
        }
        transform.Find("Rolling5").gameObject.SetActive(false);
        transform.Find("Idle").gameObject.SetActive(true);
        removeState(PlayerState.rolling);
		GetComponent<CircleCollider2D>().enabled = true;
		rollDir = Vector2.zero;
    }

    bool HasState(PlayerState checkState)
	{
		return (currentState & checkState) == checkState; // 특정 상태가 있는지 확인 (AND 연산)
	}

	public bool removeState(PlayerState state) 
	{
		currentState &= ~state;
		return false;
	}
	public bool addState(PlayerState state)
	{
		if (state == PlayerState.rolling)
		{
			// 구르기 중이면 리턴
			if (HasState(PlayerState.rolling)) 
			{
				return false;
			}
			// 구르기조건
			if (HasState(PlayerState.move) && transform.Find("Idle").gameObject.activeSelf && curStamina >= 0.3f)
			{
                if (HasState(PlayerState.meleeAttack)) 
                {
                    MeleeAttackChargeAttack();
                    removeState(PlayerState.meleeAttack);
                }
				currentState |= state;
				rollDir = inputActions["Move"].ReadValue<Vector2>();
				float angle = Mathf.Atan2(rollDir.y, rollDir.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler(0,0,angle + 90);
				StartCoroutine(RollAnimation());
				GetComponent<CircleCollider2D>().enabled = false;
			}
			else
			{
				removeState(PlayerState.rolling);
			}
		}
		else 
		{
            currentState |= state;
        }
		return true;
	}


}
