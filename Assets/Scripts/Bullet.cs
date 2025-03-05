using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50f;
    public Vector3 direction;
    public GameObject from;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Hit Wall");
            Destroy(gameObject);
        }
    }


}