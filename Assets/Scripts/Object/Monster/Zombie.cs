using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Zombie : Mob
{
    [SerializeField] private List<Part> zombie_Parts;

    public override void Start()
    {
        EnhanceMob();
        for (int i = 0; i < zombie_Parts.Count; i++)
        {
            zombie_Parts[i].hpDelivery += Damaged;
            zombie_Parts[i].brokenPart += BrokenPart;
        }
        base.Start();
        StartingMob();
    }

    private void OnDestroy()
    {
        for (int i = 0; i < zombie_Parts.Count; i++)
        {
            zombie_Parts[i].hpDelivery -= Damaged;
            zombie_Parts[i].brokenPart -= BrokenPart;
        }
    }

    public void BrokenPart(string partName)
    {
        if (partName != "" && hp > 0)
        {
            if (partName == "Head")
            {
                Damaged(100);
            }
            else if (partName == "Leg")
            {
                speed /= 2;
            }
            else if (partName == "Arm")
            {
                damage /= 2;
            }
        }
    }
    public void EnhanceMob()
    {
        if (GameManager.instance.MyGameLevel == 2)
        {
            speed += 0.7f;
            hp += 2f;
            damage += 1;
        }
        else if (GameManager.instance.MyGameLevel == 3)
        {
            speed += 1.2f;
            hp += 4f;
            damage += 2;
            for (int i = 0; i < zombie_Parts.Count; i++)
            {
                zombie_Parts[i].MyHp = (float)(hp / 3);
            }
        }
        speed += UnityEngine.Random.Range(0.0f, 0.5f);
    }
}
