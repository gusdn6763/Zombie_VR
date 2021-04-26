using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinRanger : Mob
{
    private CapsuleCollider capsuleCollider;

    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform arrowPos;
    Animation test;

    public override void Awake()
    {        
        base.Awake();
        capsuleCollider = GetComponent<CapsuleCollider>();
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
    public void EnhanceMob()
    {
        if (GameManager.instance.MyGameLevel == 2)
        {
            speed = 1f;
            hp = 6f;
            damage = 2;
        }
        else if (GameManager.instance.MyGameLevel == 3)
        {
            speed = 2f;
            hp = 9f;
            damage = 3;
        }
        speed += UnityEngine.Random.Range(0.0f, 0.5f);
    }

    public void ShootArrow()
    {
        Instantiate(arrow, arrowPos.position, arrowPos.rotation);
    }
}
