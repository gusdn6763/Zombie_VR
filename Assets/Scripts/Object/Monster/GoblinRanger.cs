using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinRanger : Mob
{
    private CapsuleCollider capsuleCollider;

    [SerializeField] private Arrow arrow;
    [SerializeField] private Transform arrowPos;
    [SerializeField] private Transform[] points;

    private Arrow grabingArrow;

    public override void Awake()
    {        
        base.Awake();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }
    public override void Start()
    {
        base.Start();
        EnhanceMob();
        StartingMob();
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

    public void MakeArrow()
    {
        if (grabingArrow == null)
        {
            grabingArrow = Instantiate(arrow, arrowPos.position, arrowPos.rotation, arrowPos);
            grabingArrow.shotPos = arrowPos;
        }
    }

    /// <summary>
    /// 애니메이션
    /// </summary>
    public void ShootArrow()
    {
        grabingArrow.ArrowShoot(damage);
    }
    public void MoveToPoint()
    {
        if (points.Length != 0)
        {
            StopAllCoroutines();
            StartCoroutine(MoveToPointCoroutine());
        }
    }
    public IEnumerator MoveToPointCoroutine()
    {
        isAttack = false;
        int i = points.Length;
        bool test = false;
        while (!test)
        {
            animator.SetBool(Constant.attack, isAttack);
            transform.position = Vector3.MoveTowards(transform.position,
                points[i].position, 1 * Time.deltaTime);
            if (Vector3.Distance(transform.position, points[i].position) <= 1f)
            {
                StartingMob();
                test = false;
                i--;
                if (i == 0)
                {
                    i = points.Length;
                }
            }
            yield return null;
        }
    }
}
