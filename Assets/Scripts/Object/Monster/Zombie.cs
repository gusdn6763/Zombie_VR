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
            zombie_Parts[i].hpDelivery += partDamegaed;
        }
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        if (enemyStatus == CharacterStatus.TRACE)
        {
            animator.SetFloat(Constant.speed, speed);
        }
    }

    public void partDamegaed(int damage, string partName = "")
    {
        base.Damaged(damage);
        if (partName != "")
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
            speed = 1.5f;
            hp = 4f;
            damage = 2;
        }
        else if (GameManager.instance.MyGameLevel == 3)
        {
            speed = 2f;
            hp = 6f;
            damage = 3;
            for (int i = 0; i < zombie_Parts.Count; i++)
            {
                zombie_Parts[i].MyHp = (float)(hp / 3);
            }
        }
        speed += UnityEngine.Random.Range(0.0f, 0.5f);
    }
}
