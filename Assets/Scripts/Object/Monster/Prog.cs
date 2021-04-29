using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prog : Mob
{
    private CapsuleCollider capsuleCollider;
    private MonsterSound monsterSound;

    public override void Awake()
    {
        base.Awake();
        capsuleCollider = GetComponent<CapsuleCollider>();
        monsterSound = GetComponent<MonsterSound>();
    }

    public override void Start()
    {
        EnhanceMob();
        base.Start();
    }


    public override void Damaged(int damage, Vector3 positon)
    {
        base.Damaged(damage, positon);
        animator.SetTrigger(Constant.hit);
    }

    public override void Die()
    {
        base.Die();
        capsuleCollider.enabled = false;
    }
}
