using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinRanger : Mob
{
    private CapsuleCollider capsuleCollider;

    [SerializeField] private Transform arrowPos;
    [SerializeField] private Transform arrowLeftPos;
    [SerializeField] private Transform[] points;

    [SerializeField] private Arrow[] grabingArrows;
    private Arrow currentArrow;

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
        //StartCoroutine(GoblinStart());
    }

    public override void Update()
    {
        base.Update();
        transform.LookAt(Player.instance.transform);
    }
    public IEnumerator GoblinStart()
    {
        while(true)
        {
            if (GameManager.instance.gameStarting)
            {
                yield return new WaitForSeconds(9f);
                StartingMob();
            }
            yield return null;
        }
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
        for(int i = 0; i < grabingArrows.Length; i++)
        {
            if (!grabingArrows[i].gameObject.activeSelf && currentArrow == null)
            {
                currentArrow = grabingArrows[i];
                print(currentArrow);
                grabingArrows[i].gameObject.SetActive(true);
                return;
            }
        }
        //grabingArrow = Instantiate(arrow, arrowPos.position, arrowPos.rotation, arrowPos);
    }

    public void TargettingArrow()
    {
        currentArrow.transform.position = new Vector3(arrowLeftPos.position.x, arrowLeftPos.position.y +0.45f, arrowLeftPos.position.z);
        Vector3 test = transform.eulerAngles;
        currentArrow.transform.eulerAngles = new Vector3(test.x - 20f, test.y, test.z);
    }

    /// <summary>
    /// 애니메이션
    /// </summary>
    public void ShootArrow()
    {
        currentArrow.ArrowShoot(damage);
        currentArrow = null;
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
