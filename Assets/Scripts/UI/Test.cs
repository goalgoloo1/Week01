//using UnityEngine;

//public class Test : MonoBehaviour
//{
//    [SerializeField] private LayerMask layerMask;

//    private void Start()
//    {
//        Mesh mesh = new Mesh();
//        GetComponent<MeshFilter>().mesh = mesh;

//        float fov = 360f;
//        Vector3 origin = Vector3.zero;
//        int rayCount = 360;
//        float angle = 0f;
//        float angleIncrease = fov / rayCount;
//        float viewDistance = 5f;

//        Vector3[] vertices = new Vector3[rayCount + 2];
//        Vector2[] uv = new Vector2[vertices.Length];
//        int[] triangles = new int[rayCount*3];

//        vertices[0] = origin;

//        int vertexIndex = 1;
//        int triangleIndex = 0;
//        for (int i = 0; i <= rayCount; i++) 
//        {
//            Vector3 vertex;
//            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);
//            if (raycastHit2D.collider == null)
//            {
//                Debug.Log("no hit");
//                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
//            }
//            else 
//            {
//                Debug.Log("hit");
//                vertex = raycastHit2D.point;
//            }
//            vertices[vertexIndex] = vertex;

//            if (i > 0) 
//            {
//                triangles[triangleIndex + 0] = 0;
//                triangles[triangleIndex + 1] = vertexIndex - 1;
//                triangles[triangleIndex + 2] = vertexIndex;
//                triangleIndex += 3;
//            }
//            vertexIndex++;
//            angle -= angleIncrease;
//        }

//        mesh.vertices = vertices;
//        mesh.uv = uv;
//        mesh.triangles = triangles;
//    }

//    // 각도를 입력받으면 방향벡터로 변환하는 함수
//    Vector3 GetVectorFromAngle(float angle) 
//    {
//        float angleRad = angle * (Mathf.PI / 180f); // 각도를 라디안으로 변환하는 공식
//        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
//    }



//}
