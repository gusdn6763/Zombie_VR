using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    [SerializeField] private float hp;

    private BoxCollider collObject;
    public Action<int> hpDelivery;
    public GameObject[] brokeObjects;
    public string partName;


    private void Awake()
    {
        collObject = GetComponent<BoxCollider>();
    }

    public void BrokenPart()
    {
        for (int i = 0; i < brokeObjects.Length; i++)
        {
            Destroy(brokeObjects[i]);
        }
    }

    public void Damaged(int damage)
    {
        hp -= damage;
        hpDelivery(damage);
        if (hp <= 0)
        {
            BrokenPart();
        }
    }

}
