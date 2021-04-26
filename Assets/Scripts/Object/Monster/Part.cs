using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    [SerializeField] private float hp;

    public BoxCollider[] collObjects;
    public GameObject[] brokeObjects;

    public Action<int> hpDelivery;
    public Action<string> brokenPart;

    public string partName;
    public bool broken = false;

    public float MyHp { get; set; }

    public void Damaged(int damage)
    {
        hp -= damage;
        if (!broken)
        {
            hpDelivery(damage);
            if (hp <= 0)
            {
                broken = true;
                brokenPart(partName);
                for (int i = 0; i < brokeObjects.Length; i++)
                {
                    Destroy(brokeObjects[i]);
                    collObjects[i].enabled = false;                    
                }
            }
        }
    }



}