using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    void Start()
    {
        bullet = Resources.Load<GameObject>("Prefabs/Bullet");
    }

    void Update()
    {
        
    }

    public void fire() 
    {
        Instantiate(bullet, transform.position + transform.right * 0.7f, transform.rotation);
    }
}
