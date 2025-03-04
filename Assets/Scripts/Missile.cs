using UnityEngine;
using UnityEngine.UIElements;

public class Missile : MonoBehaviour
{
    int speed = 50;
    public Vector3 spawnpoint;
    public GameObject _redzone;
    void Start()
    {
        
    }

    void Update()
    {
        if (transform.position.y < spawnpoint.y) 
        {
            Destroy(gameObject);
            Destroy(_redzone);
        }
        transform.Translate(-transform.up * speed * Time.deltaTime);
    }

    
}
