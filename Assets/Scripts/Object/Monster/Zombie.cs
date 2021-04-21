using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Zombie : Mob
{
    [SerializeField] private List<Part> zombie_Parts;

    private Rigidbody rigi;

    public override void Start()
    {
        base.Start();

        speed += UnityEngine.Random.Range(0, 2);
        for (int i = 0; i < zombie_Parts.Count; i++)
        {
            zombie_Parts[i].hpDelivery += partDamegaed;
        }
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


}
