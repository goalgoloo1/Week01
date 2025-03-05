using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : Character
{
    public GameObject gun;
    public Transform firePoint;
    public Canvas_Script canvas;
    public GameManager gameManager;
    public bool isCanSave = false;
    void Start()
    {
        movespeed = 10;
        hp = 2;
        gun = transform.GetChild(0).gameObject;
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
        canvas = GameObject.FindFirstObjectByType<Canvas_Script>();
    }

    void Update()
    {
        if (!gameManager.isgameover) 
        {
            // 이동
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector2.up * movespeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector2.left * movespeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector2.down * movespeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector2.right * movespeed * Time.deltaTime, Space.World);
            }

            // 플레이어 각도
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector2 direction = (mousePosition - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));

            // 발사
            if (Input.GetMouseButtonDown(0))
            {
                gun.GetComponent<Gun>().fire();
            }

            // 플레이어 체력이 적으면 UI표시
            if (hp < 2 && hp > 0)
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
                    transform.Find("AidKit_Indicator").gameObject.SetActive(false);
                }
            }

            // 자해(테스트용)
            if (Input.GetKeyDown(KeyCode.Z))
            {
                hp--;
            }

            // 사망
            if (hp < 1)
            {
                gameManager.Gameover();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 살리기
        if (isCanSave && collision.gameObject.CompareTag("Patient"))
        {
            isCanSave = false;
            transform.Find("AidKit_Indicator").gameObject.SetActive(false);
            gameManager.score += 10;

            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 구상과 겹쳐있을 때
        if (isCanSave && collision.gameObject.CompareTag("Patient"))
        {
            isCanSave = false;
            transform.Find("AidKit_Indicator").gameObject.SetActive(false);
            gameManager.score += 10;

            canvas.score.GetComponent<TextMeshProUGUI>().text = "score : " + gameManager.score;

            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && collision.gameObject.GetComponent<Bullet>().from.tag != gameObject.tag)
        {
            hp--;
            collision.gameObject.SetActive(false);
        }
    }
}
