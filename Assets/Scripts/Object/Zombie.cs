using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Zombie : Mob
{
    [SerializeField] private Part[] zombie_Parts;

    public override void Start()
    {
        base.Start();

        speed = UnityEngine.Random.Range(0, 4);
        for (int i = 0; i < zombie_Parts.Length; i++)
        {
            zombie_Parts[i].hpDelivery += Damaged;
        }
    }

    public override void Damaged(int damage)
    {
        base.Damaged(damage);
    }

}
