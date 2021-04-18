using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    [SerializeField] private float hp;

    public Action<int> hpDelivery;
    public void BrokenPart()
    {
        Destroy(this.gameObject);
    }


}
