using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    [SerializeField] private float hp;

    public BoxCollider[] collObjects;
    public GameObject[] brokeObjects;

    public Action<int, Vector3> hpDelivery;
    public Action<string> brokenPart;

    public string partName;
    public bool broken = false;

    public float MyHp { get; set; }


    public void Damaged(int damage, Vector3 position)
    {
        hp -= damage;
        if (!broken)
        {
            hpDelivery(damage, position);
 
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
