using UnityEngine;

public class Bullet : MonoBehaviour
{
    int movespeed = 50;
    GameObject player;
    void Start()
    {
        player = GameObject.FindFirstObjectByType<Player>().gameObject;
    }

    void Update()
    {
        transform.Translate(Vector2.down * movespeed * Time.deltaTime);
        if (50 < Vector3.Distance(player.transform.position, transform.position))
        {
            Debug.Log("destroy");
            Destroy(gameObject);
        }
    }
}
