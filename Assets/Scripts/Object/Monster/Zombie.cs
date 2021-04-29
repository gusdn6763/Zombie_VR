using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Zombie : Mob
{
    [SerializeField] private List<Part> zombie_Parts;
    private MonsterSound monsterSound;

    public override void Awake()
    {
        base.Awake();
        monsterSound = GetComponent<MonsterSound>();
    }

    public override void Start()
    {
        EnhanceMob();
        for (int i = 0; i < zombie_Parts.Count; i++)
        {
            zombie_Parts[i].hpDelivery += Damaged;
            zombie_Parts[i].brokenPart += BrokenPart;
        }
        base.Start();
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
                Damaged(100, new Vector3(0,0,0));
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
