using UnityEngine;

public class DonutMesh : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    void Start()
    {
        //int innerverteices = 4;  // 안쪽도넛의 원을 이루는 점 개수
        //int outerverteices = innerverteices*2;  // 바깥도넛의 원을 이루는 점 개수
        //float outerRadius = 2f;  // 바깥 원 반지름
        //float innerRadius = 1f;  // 안쪽 원 반지름
        //float innerAngleIncrease = 360 / innerverteices;
        //float outerAngleIncrease = 360 / outerverteices;

        // Mesh mesh = new Mesh();
        //GetComponent<MeshFilter>().mesh = mesh;

        //float fov = 360f;
        //Vector3 origin = Vector3.zero;
        //float angle = 0f;
        //float angleIncrease = fov / innerverteices;

        //int vertexCount = innerverteices * 2; // 안쪽 + 바깥쪽 원 점 개수
        //Vector3[] vertices = new Vector3[vertexCount];
        //int[] triangles = new int[innerverteices * 6]; // 삼각형 개수
        //Vector2[] uv = new Vector2[vertexCount];

        //float angleStep = 360f / innerverteices;
        //for (int i = 0; i < innerverteices; i++)
        //{
        //}

        //mesh.vertices = vertices;
        //mesh.triangles = triangles;
        //mesh.uv = uv;
        //mesh.RecalculateNormals();
    }
}
