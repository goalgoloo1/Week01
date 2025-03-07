using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.UIElements;

public class FieldOfView_Script : MonoBehaviour
{
    private Mesh mesh;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Update()
    {
        float fov = 90f;
        Vector3 origin = Vector3.zero;
        int rayCount = 50;
        float angle = 0f;
        float angleIncrease = fov / rayCount;
        float viewDistance = 50f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex = origin + UtilsClass.GetVectorFromAngle(angle) * viewDistance;

            /*
            if (raycastHit2D.collider == null)
            {
                // 아무것도 부딪치지 않음
                vertex = origin + UtilsClass.GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                // 장애물과 부딪힘
                vertex = raycastHit2D.point;
            }
            */

            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}
