using Unity.VisualScripting;
using UnityEngine;

public class BulletLine : GameObjectBase
{
    LineRenderer lineRenderer;
    public GameObject parentBullet;
    void OnEnable() 
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;  // ���� ���� �β�
        lineRenderer.endWidth = 0.05f;    // ���� �� �β�
        lineRenderer.startColor = Color.white;  // ���� ���� ����
        lineRenderer.endColor = Color.white;    // ���� �� ����
        //lineRenderer.material = new Material(Shader.Find("Sprites/Default"));  // �⺻ ���̴�
    }
    private void Update()
    {

        Color startColor = lineRenderer.startColor;
        startColor.a -= Time.deltaTime * 3;
        lineRenderer.startColor = startColor;

        Color endColor = lineRenderer.endColor;
        if (!parentBullet) 
        {
            endColor.a -= Time.deltaTime * 3;
            lineRenderer.endColor = endColor;
        }


        //float startWidth = lineRenderer.startWidth;
        //startWidth -= 0.1f * Time.deltaTime;
        //Mathf.Clamp(startWidth, 0f, 2f);
        //lineRenderer.startWidth = startWidth;
        //float endWidth = lineRenderer.endWidth;
        //endWidth -= 0.1f * Time.deltaTime;
        //Mathf.Clamp(endWidth, 0f, 2f);
        //lineRenderer.endWidth = endWidth;

        if (endColor.a < 0)
        {
            Destroy(gameObject);
        }
    }
}
