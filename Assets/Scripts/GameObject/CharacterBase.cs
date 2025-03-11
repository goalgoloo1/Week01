using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : GameObjectBase
{
    public int hp;
    public float moveSpeed;
    protected float maxStamina;
    protected float curStamina;
    protected float beforeStamina;
    protected float fastMoveMultiply;

    public float doingGauge;
    public List<GameObject> something;
    public bool onSomething = false;

    public Weapon weaponScript;
    public GameObject weapon;
    protected GameObject deadParticle;

    public float stunTime;
    protected void FindWeapon()
    {
        Transform hand = transform.Find("Hand");
        if (!hand) 
        {
            Debug.Log("�� ��ã��");
        }
        if (hand.childCount == 1)
        {
            Debug.Log(transform.name + " ������");
            weapon = hand.GetChild(0).gameObject;
            weaponScript = weapon.GetComponent<Weapon>();
        }
        else
        {
            Debug.Log(transform.name + " �Ѿ���");
            weapon = Instantiate(Resources.Load<GameObject>("Prefabs/WeaponPrefabs/HandGun"));
            if (!weapon) 
            {
                Debug.Log("�Ѹ�ã��");
            } 

            weapon.transform.SetParent(hand);
            weaponScript = weapon.GetComponent<Weapon>();

            Debug.Log(transform.name);
        }
        Debug.Log(weaponScript.GetType());
    }

    public void takeDamage(Vector2 direction, float power, int damage) 
    {
        Instantiate(deadParticle, transform.position, transform.rotation);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb) rb.AddForce(direction.normalized * power, ForceMode2D.Impulse);
        hp -= damage;
        stunTime += damage;
    }

    protected void Dead()
    {
        Instantiate(deadParticle, transform.position, transform.rotation);
        FindWeapon();
        if (Random.Range(0, 4) == 3) 
        {
            DropWeapon();
        }
        Destroy(gameObject);
    }

    protected void DropWeapon() 
    {
        if (weapon)
        {
            weapon.transform.SetParent(null);
            weapon.GetComponent<Weapon>().onHandIMG.SetActive(false);
            weapon.GetComponent<Weapon>().onGroundIMG.SetActive(true);
            weapon.GetComponent<BoxCollider2D>().enabled = true;
        }
        else 
        {
            Debug.Log("������⸦ �Ȱ����� ����");
        }
    }

    protected void TakeWeapon(GameObject dropedWeapon) 
    {
        dropedWeapon.transform.SetParent(transform.Find("Hand"));
        dropedWeapon.GetComponent<Weapon>().onHandIMG.SetActive(true);
        dropedWeapon.GetComponent<Weapon>().onGroundIMG.SetActive(false);
        dropedWeapon.GetComponent<BoxCollider2D>().enabled = false;
        dropedWeapon.transform.position = weapon.transform.position;
        dropedWeapon.transform.rotation = weapon.transform.rotation;
        stunTime = 0.5f;
        DropWeapon();
        FindWeapon();
    }
}
