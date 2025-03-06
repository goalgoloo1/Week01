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
        GameObject fire_bullet = Instantiate(bullet, transform.position, Quaternion.identity);
        fire_bullet.GetComponent<Bullet>().SetDirection(-transform.up);
        fire_bullet.GetComponent<Bullet>().from = transform.parent.gameObject.tag;
        //Instantiate(bullet, transform.position, transform.parent.rotation);
    }
}
