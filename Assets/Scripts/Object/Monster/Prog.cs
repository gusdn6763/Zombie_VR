using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prog : Mob
{
    public override void Damaged(int damage, Vector3 positon)
    {
        base.Damaged(damage, positon);
        animator.SetTrigger(Constant.hit);
    }

}
