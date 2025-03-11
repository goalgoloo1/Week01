using UnityEngine;

public class Bullet : SpawnManager
{
    Rigidbody2D rb;
    float bulletSpeed = 300f;
    public Vector2 direction;

    GameObject bulletLine;
    Vector2 beforePos;
    Vector2 currentPos;

    LineRenderer lineRenderer;
    int pointCount = 0;

    public GameObject fireFrom;
    bool ishit = false;
    float power = 10;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = -transform.up;
        bulletLine = Resources.Load<GameObject>("Prefabs/BulletLine");
        currentPos = transform.position;
    }
    private void Start()
    {
        lineRenderer = Instantiate(bulletLine).GetComponent<LineRenderer>();
        lineRenderer.gameObject.GetComponent<BulletLine>().parentBullet = gameObject;
        pointCount++;
        lineRenderer.positionCount = pointCount;
        lineRenderer.SetPosition(pointCount-1, transform.position);
    }
    void Update()
    {
        if (Vector2.Distance(fireFrom.transform.position, transform.position) > 50) 
        {
            Destroy(gameObject);
        }

        rb.linearVelocity = direction * bulletSpeed;

        beforePos = currentPos;
        currentPos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(beforePos, (currentPos - beforePos).normalized, Vector2.Distance(beforePos, currentPos));
        if (hit && hit.collider.tag != fireFrom.tag && !ishit && hit.collider.GetComponent<CharacterBase>()) 
        {
            ishit = true;
            transform.position = hit.point;
            bulletSpeed = 0;
            hit.collider.GetComponent<CharacterBase>().takeDamage(direction, power, 1);
            Destroy(gameObject);
        }

        pointCount++;
        lineRenderer.positionCount = pointCount;
        lineRenderer.SetPosition(pointCount-1, transform.position);
    }
}
