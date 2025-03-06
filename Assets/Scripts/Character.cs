using UnityEngine;

public class Character : MonoBehaviour
{
    public int movespeed;
    public int hp;
    protected int runMultiply = 1;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            Debug.Log("dddddd");
            movespeed /= 2;
        }
    }

    public void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            Debug.Log("dddddd");
            movespeed *= 2;
        }
    }
}

