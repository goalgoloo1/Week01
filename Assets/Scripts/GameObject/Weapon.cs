using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Weapon : GameObjectBase
{
    public float fireRate;
    public float rateGauge;
    public int maxMagazin;
    public int remainBullet;
    public GameObject onGroundIMG;
    public GameObject onHandIMG;
    public virtual void Fire() 
    {
        Debug.Log("Gun Fire");
    }

    protected void ShowFlash() 
    {
        StartCoroutine(ShowFlash_());
    }
    IEnumerator ShowFlash_()
    {
        transform.Find("Flash").gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        transform.Find("Flash").gameObject.SetActive(false);
    }

    private void Update()
    {
        
    }
    public static Vector2 AngleToDirection(float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad; // 각도를 라디안으로 변환
        return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}
