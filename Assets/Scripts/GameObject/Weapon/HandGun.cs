using TMPro;
using UnityEngine;

public class HandGun : Weapon
{
    [SerializeField] GameObject bullet;
    private void OnEnable()
    {
        fireRate = 0.2f;
        maxMagazin = 12;
        remainBullet = maxMagazin;
        bullet = Resources.Load<GameObject>("Prefabs/Bullet");
        onGroundIMG = transform.Find("HandGun_OnGround").gameObject;
        onHandIMG = transform.Find("HandGun_Ingame").gameObject;
    }

    private void Update()
    {
        if (rateGauge < fireRate) 
        {
            rateGauge += Time.deltaTime;
        }
        if (onGroundIMG.activeSelf && remainBullet == 0) 
        {
            Destroy(gameObject);
        }
    }

    public override void Fire()
    {
        Debug.Log("HandGun Fire!");
        remainBullet--;
        ShowFlash();

        // UI에 정보 전달
        GameObject.FindFirstObjectByType<MainCanvas>().ShowMagazin(this);

        rateGauge = 0;
        GameObject firedBullet = Instantiate(bullet, transform.position + -transform.parent.parent.up * 1, transform.rotation);
        firedBullet.GetComponent<Bullet>().firePosition = transform.position;
        firedBullet.GetComponent<Bullet>().fireFrom = gameObject;
    }
}
