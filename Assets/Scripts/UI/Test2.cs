using UnityEngine;

public class DonutMesh : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    void Start()
    {
        //int innerverteices = 4;  // ���ʵ����� ���� �̷�� �� ����
        //int outerverteices = innerverteices*2;  // �ٱ������� ���� �̷�� �� ����
        //float outerRadius = 2f;  // �ٱ� �� ������
        //float innerRadius = 1f;  // ���� �� ������
        //float innerAngleIncrease = 360 / innerverteices;
        //float outerAngleIncrease = 360 / outerverteices;

        // Mesh mesh = new Mesh();
        //GetComponent<MeshFilter>().mesh = mesh;

        //float fov = 360f;
        //Vector3 origin = Vector3.zero;
        //float angle = 0f;
        //float angleIncrease = fov / innerverteices;

        //int vertexCount = innerverteices * 2; // ���� + �ٱ��� �� �� ����
        //Vector3[] vertices = new Vector3[vertexCount];
        //int[] triangles = new int[innerverteices * 6]; // �ﰢ�� ����
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
