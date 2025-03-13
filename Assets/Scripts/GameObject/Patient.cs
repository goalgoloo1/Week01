using System;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;

public class Patient : GameObjectBase
{
    public Transform fillTransform;
    public event Action death;
    public float doingGaugeNeeded = 2f;
    void Start()
    {
        fillTransform = transform.Find("Canvas/TimerBar/Fill");
        death += Dead;
    }
    void Update()
    {
        //currentHealth -= Time.deltaTime;
        //fillTransform.localScale = new Vector3(currentHealth / MaxHealth, 1, 1);
        //if (currentHealth <= 0) 
        //{
        //    death?.Invoke();
        //}
    }
    void Dead() 
    {
        Destroy(gameObject);
    }
}
