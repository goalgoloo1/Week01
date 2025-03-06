using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
public class Player : Character
{
    public enum PlayerState 
    {
        Walk,
        Run,
        Save
    }
    public PlayerState currentState = 0;

    public GameObject gun;
    public Transform firePoint;
    public Canvas_Script canvas;
    public GameManager gameManager;
    public ParticleSystem deathParticle;
    public ParticleSystem healParticle;

    GameObject targetPatient;

    public bool isHaveAdkit = false;

    public float stamina;
    public float holdKeyTime = 0f;
    public bool isCanSave = false;
    void Start()
    {
        movespeed = 10;
        hp = 2;
        stamina = 1.5f;
        runMultiply = 1;
        gun = transform.GetChild(0).gameObject;
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
        canvas = GameObject.FindFirstObjectByType<Canvas_Script>();
    }
    void Update()
    {
        if (!gameManager.isgameover)
        {
            // 플레이어 상태변화
            if (Input.GetKeyDown(KeyCode.LeftShift) && stamina > 0.3f) 
            {
                currentState = PlayerState.Run;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift) && currentState == PlayerState.Run) 
            {
                currentState = PlayerState.Walk;
            }

            switch (currentState) 
            {
                case PlayerState.Walk:
                    runMultiply = 1;
                    break;
                case PlayerState.Run:
                    runMultiply = 1.5f;
                    break;
                case PlayerState.Save:
                    runMultiply = 0;
                    break;
            }

            // stamina
            if (currentState == PlayerState.Run)
            {
                if (stamina > 0)
                {
                    stamina -= Time.deltaTime;
                }
                else 
                {
                    currentState = PlayerState.Walk;
                }
            }
            else 
            {
                stamina += Time.deltaTime/2;
            }
            stamina = Mathf.Clamp(stamina, 0, 1.5f);
            // 스태바로 정보전달
            canvas.staminaBar.transform.localScale = new Vector3(stamina/1.5f, 1, 1);

            // 이동
            if (currentState != PlayerState.Save)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    transform.Translate(Vector2.up * movespeed * Time.deltaTime * runMultiply, Space.World);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    transform.Translate(Vector2.left * movespeed * Time.deltaTime * runMultiply, Space.World);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    transform.Translate(Vector2.down * movespeed * Time.deltaTime * runMultiply, Space.World);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    transform.Translate(Vector2.right * movespeed * Time.deltaTime * runMultiply, Space.World);
                }
            }

            if (isCanSave && targetPatient) 
            {
                if (Input.GetKey(KeyCode.E))
                {
                    targetPatient.GetComponent<Patient_Script>().issaving = 0;
                    currentState = PlayerState.Save;
                    holdKeyTime += Time.deltaTime;

                    if (holdKeyTime >= 1f) 
                    {
                        gameManager.score += 1;
                        TriggerPatientHeal();
                        Destroy(targetPatient);
                        holdKeyTime = 0;
                        isCanSave = false;
                        currentState = PlayerState.Walk;
                        transform.Find("AidKit_Indicator").gameObject.SetActive(false);
                        isHaveAdkit = false;
                    }
                }
                else 
                {
                    holdKeyTime = 0;
                    currentState = PlayerState.Walk;
                    targetPatient.GetComponent<Patient_Script>().issaving = 1;
                }

                if (targetPatient) 
                {
                    targetPatient.GetComponent<Patient_Script>().fill.transform.localScale = new Vector3(1, holdKeyTime / 1, 1);
                }
            }

            // 플레이어 각도
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector2 direction = (mousePosition - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
            // 발사
            if (Input.GetMouseButtonDown(0) && currentState != PlayerState.Save)
            {
                gun.GetComponent<Gun>().fire();
            }
            // 플레이어 체력이 적으면 UI표시
            if (hp == 1)
            {
                canvas.lowHp_UI.SetActive(true);
            }
            else
            {
                canvas.lowHp_UI.SetActive(false);
            }
            // 자힐
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (transform.Find("AidKit_Indicator").gameObject.activeSelf && hp < 2)
                {
                    hp++;
                    TriggerSelfHeal();
                    transform.Find("AidKit_Indicator").gameObject.SetActive(false);
                    isHaveAdkit = false;
                }
            }
            //// 자해(테스트용)
            //if (Input.GetKeyDown(KeyCode.Z))
            //{
            //    hp--;
            //}
            // 사망
            if (hp < 1)
            {
                gameManager.Gameover();
                //Color c = gameObject.GetComponent<SpriteRenderer>().color;
                //c.a = 0;
                //gameObject.GetComponent<SpriteRenderer>().color = c;
                TriggerDeath();
                if (gameObject != null)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
    void TriggerDeath() 
    {
        if (deathParticle != null)
        {
            ParticleSystem death = Instantiate(deathParticle, transform.position, Quaternion.identity);
            death.Play();
            Destroy(death.gameObject, 2f);
        }
    }
    

    void TriggerSelfHeal()
    {
        if (healParticle != null)
        {
            ParticleSystem heal = Instantiate(healParticle, transform.position, Quaternion.identity);
            heal.Play();
            Destroy(heal.gameObject, 2f);
        }
    }
    void TriggerPatientHeal()
    {
        if (healParticle != null)
        {
            ParticleSystem heal = Instantiate(healParticle, targetPatient.transform.position, Quaternion.identity);
            heal.Play();
            Destroy(heal.gameObject, 2f);
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        //환자 살리기
        if (isHaveAdkit && collision.gameObject.CompareTag("Patient"))
        {
            isCanSave = true;
            targetPatient = collision.gameObject;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == targetPatient) 
        {
            isCanSave = false;
            holdKeyTime = 0f;
            targetPatient = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && collision.gameObject.GetComponent<Bullet>().from != gameObject.tag)
        {
            hp--;
            collision.gameObject.SetActive(false);
        }

    }
}