using UnityEngine;
using UnityEngine.UIElements;

public class Player : Character
{
    public GameObject gun;
    public Transform firePoint;
    bool isLowhp = false;
    public Canvas canvas;
    public bool isCanSave = false;
    void Start()
    {
        movespeed = 10;
        hp = 2;
        gun = transform.GetChild(0).gameObject;
    }

    void Update()
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
        Transform lowHp_ui = canvas.transform.Find("LowHP_UI");
        if (hp < 2)
        {
            lowHp_ui.gameObject.SetActive(true);
        }
        else 
        {
            lowHp_ui.gameObject.SetActive(false);
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
    }
}
