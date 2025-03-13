using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class SpawnManager : MonoBehaviour
{
    // 플레이어, 카메라
    public GameObject player;
    public GameObject mainCamera;
    // UI
    public GameObject mainCanvas;
    //적
    public GameObject enemy;
    // 화살표
    public GameObject arrowPrefab;
    public float indicatorRange = 300f;
    // 캠
    private Camera cam;
    // 환자
    public GameObject patient;
    // 구급킷
    public GameObject AidKit;
    public int score;

    private void Awake()
    {
        Screen.SetResolution(900, 900, false);
    }

    void Start()
    {
        FindObject();
        if (GetType() == typeof(SpawnManager))
        {
            mainCamera = Instantiate(mainCamera);
            mainCanvas = Instantiate(mainCanvas);
            player = Instantiate(player);
            //Instantiate(enemy);
        }
        cam = Camera.main;
    }

    float cooltime = 5f;
    float curcooltime = 0f;
    void Update()
    {
        UpdateArrowIndicators();
        //if (GameObject.FindGameObjectsWithTag("Patient").Length == 0) 
        //{
        //    Vector2 RandomVector = new Vector2(Random.Range(-40f, 40f), Random.Range(-40f, 40f));
        //    Instantiate(patient, RandomVector, Quaternion.Euler(Vector3.zero));
        //}
        //if (GameObject.FindGameObjectsWithTag("Emergency Kit").Length == 0)
        //{
        //    Vector2 RandomVector = new Vector2(Random.Range(-40f, 40f), Random.Range(-40f, 40f));
        //    Instantiate(AidKit, RandomVector, Quaternion.Euler(Vector3.zero));
        //}
        curcooltime += Time.deltaTime;
        if (curcooltime > cooltime) 
        {
            Vector2 RandomVector = new Vector2(Random.Range(-40f, 40f), Random.Range(-40f, 40f));
            Instantiate(enemy, RandomVector, Quaternion.Euler(Vector3.zero));
            curcooltime = 0;
        }
        mainCanvas.GetComponent<MainCanvas>().score.GetComponent<TextMeshProUGUI>().text = "점수 : " + score.ToString();
        if (player.GetComponent<Player>().hp == 1) 
        {
            mainCanvas.GetComponent<MainCanvas>().lowHp_UI.SetActive(true);
        }
    }

    public void FindObject()
    {
        player = Resources.Load<GameObject>("Prefabs/PlayerPrefabs/Player");
        if (!player) Debug.Log("플레이어 못찾음");
        mainCamera = Resources.Load<GameObject>("Prefabs/Main_Camera");
        if (!mainCamera) Debug.Log("카메라 못찾음");
        enemy = Resources.Load<GameObject>("Prefabs/EnemyPrefabs/Enemy");
        if (!enemy) Debug.Log("Enemy 못찾음");
        mainCanvas = Resources.Load<GameObject>("Prefabs/UI/Canvas");
        arrowPrefab = Resources.Load<GameObject>("Prefabs/arrowPrefab");
        patient = Resources.Load<GameObject>("Prefabs/Patient");
        AidKit = Resources.Load<GameObject>("Prefabs/EmergencyKit");
    }

    void UpdateArrowIndicators()
    {
        // 환자 찾기
        GameObject[] patients = GameObject.FindGameObjectsWithTag("Patient");
        GameObject[] aidKits = GameObject.FindGameObjectsWithTag("Emergency Kit");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (patients.Length == 0)
        {
            Debug.Log("환자 없음");
        }
        else 
        {
            foreach (GameObject patient in patients)
            {
                CreateArrow(patient.transform);
            }
        }
        if (aidKits.Length == 0)
        {
            Debug.Log("구상 없음");
        }
        else
        {
            foreach (GameObject aidKit in aidKits)
            {
                CreateArrow(aidKit.transform);
            }
        }
        if (enemies.Length == 0)
        {
            Debug.Log("구상 없음");
        }
        else
        {
            foreach (GameObject enemy in enemies)
            {
                CreateArrow(enemy.transform);
            }
        }
    }
    void CreateArrow(Transform target)
    {
        Vector3 targetpos = cam.WorldToViewportPoint(target.transform.position);
        Transform arrow = target.Find(target.tag + "arrow");
        // 화면 바깥에 있고, 자식에 화살표가 없으면
        if ((targetpos.x < 0 || targetpos.x > 1 || targetpos.y < 0 || targetpos.y > 1) && !arrow)
        {
            Debug.Log("asdf");
            // 화살표 생성
            GameObject newArrow = Instantiate(arrowPrefab, target.transform);
            // 화살표 이름
            newArrow.name = target.tag + "arrow";
            if (player) 
            {
                newArrow.GetComponent<ArrowDirection>().player = player;
            }
            
        }
        // 화면 안쪽이고 자식에 화살표가 있으면
        else if ((targetpos.x > 0 && targetpos.x < 1 && targetpos.y > 0 && targetpos.y < 1) && arrow)
        {
            Debug.Log("zxcv");
            Destroy(arrow.gameObject);
        }
    }
}
