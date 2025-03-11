//using System.Collections.Generic;
//using UnityEngine;

//public class ArrowIndicator : MonoBehaviour
//{
//    public Transform player; // 플레이어 참조
//    public GameObject arrowPrefab; // 화살표 프리팹
//    private List<GameObject> arrows = new List<GameObject>(); // 생성된 화살표 리스트
//    private Camera cam;
//    public float indicatorRange = 30f;

//    void Start()
//    {
//        cam = Camera.main;
//        arrowPrefab = Resources.Load<GameObject>("arrowPrefab");
//        player = GetComponent<SpawnManager>().player.transform;
//    }

//    void Update()
//    {
//        UpdateArrowIndicators();
//    }


//}
