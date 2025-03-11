using Unity.VisualScripting;
using UnityEngine;

public class BulletLine : GameObjectBase
{
    LineRenderer lineRenderer;
    public GameObject parentBullet;
    void OnEnable() 
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;  // 선의 시작 두께
        lineRenderer.endWidth = 0.05f;    // 선의 끝 두께
        lineRenderer.startColor = Color.white;  // 선의 시작 색상
        lineRenderer.endColor = Color.white;    // 선의 끝 색상
        //lineRenderer.material = new Material(Shader.Find("Sprites/Default"));  // 기본 쉐이더
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
