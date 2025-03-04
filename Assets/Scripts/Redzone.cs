using UnityEngine;

public class Redzone : MonoBehaviour
{
    float value = 0;
    GameObject missile;
    bool isfire = false;
    void Start()
    {
        transform.localScale = new Vector3(0, 0, 1);
        missile = Resources.Load<GameObject>("Prefabs/Missile");
    }

    void Update()
    {
        if (value > 1 && !isfire)
        {
            isfire = true;
            Missile m = Instantiate(missile, transform.position + new Vector3(0,30,0), transform.rotation).GetComponent<Missile>();
            m.spawnpoint = transform.position;
            m._redzone = transform.parent.gameObject;
        }
        else 
        {
            transform.localScale = new Vector3(value, value, 1);
            value += 1f * Time.deltaTime;
        }
    }
}
