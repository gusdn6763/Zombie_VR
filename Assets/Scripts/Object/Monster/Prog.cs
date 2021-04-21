using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prog : Mob
{
    public override void Damaged(int damage)
    {
        base.Damaged(damage);
        animator.SetTrigger(Constant.hit);
    }
}
