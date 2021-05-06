using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinRanger : Mob
{
    [SerializeField] private Transform grabArrowPos;
    [SerializeField] private Transform beforeAttackArrowPos;
    [SerializeField] private Transform[] avoidancePos;
    [SerializeField] private Arrow[] objectPoolArrows;

    private Arrow currentArrow;
    private int i;

    [SerializeField] private float waitTime = 2f;


    public override void Start()
    {
        monsterCollider.enabled = false;
        i = avoidancePos.Length;
        base.Start();
        StartCoroutine(GoblinStart());
    }


    public IEnumerator GoblinStart()
    {
        while (true)
        {
            if (GameManager.instance.gameStarting)
            {
                EnhanceMob();
                yield return new WaitForSeconds(10f);
                monsterCollider.enabled = true;
                GoblinAttack();
                break ;
            }
            yield return null;
        }
    }

    public void GoblinAttack()
    {
        Stop();
        animator.SetBool(Constant.move, false);
        animator.SetBool(Constant.attack, true);
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(targetPosition);
    }

    public override void Damaged(int damage, Vector3 positon)
    {
        base.Damaged(damage, positon);
        animator.SetTrigger(Constant.hit);
        if (currentArrow != null)
        {
            currentArrow.gameObject.SetActive(false);
        }
        currentArrow = null;
    }

    public void MakeArrow()
    {
        for(int i = 0; i < objectPoolArrows.Length; i++)
        {
            if (!objectPoolArrows[i].gameObject.activeSelf && currentArrow == null)
            {
                currentArrow = objectPoolArrows[i];
                currentArrow.transform.SetParent(grabArrowPos);
                objectPoolArrows[i].gameObject.SetActive(true);
                return;
            }
        }
    }

    public void TargettingArrow()
    {
        currentArrow.transform.SetParent(beforeAttackArrowPos);
        currentArrow.transform.position = new Vector3(beforeAttackArrowPos.position.x, beforeAttackArrowPos.position.y +0.45f, beforeAttackArrowPos.position.z);
        Vector3 test = transform.eulerAngles;
        currentArrow.transform.eulerAngles = new Vector3(test.x - 20f, test.y, test.z);
    }

    /// <summary>
    /// 애니메이션
    /// </summary>
    public void ShootArrow()
    {
        if (currentArrow != null)
        {
            currentArrow.ArrowShoot(damage);
            currentArrow = null;
        }
    }

    public void MoveToPoint()
    {
        if (avoidancePos.Length != 0)
        {
            if (i == 0)
            {
                i = avoidancePos.Length;
            }
            i--;
            animator.SetBool(Constant.attack, false);
            animator.SetBool(Constant.move, false);
            StartCoroutine(MoveToPointCoroutine());
        }
        else
        {
            StartCoroutine(WaitAttack());
        }    
    }


    public IEnumerator MoveToPointCoroutine()
    {
        yield return new WaitForSeconds(waitTime);
        animator.SetBool(Constant.move, true);
        enemyStatus = CharacterStatus.TRACE;
        TraceTarget(avoidancePos[i].position);
        bool test = false;
        while (!test)
        {
            if (Vector3.Distance(transform.position, avoidancePos[i].position) <= 1f)
            {
                test = true;
                animator.SetBool(Constant.move, false);
                enemyStatus = CharacterStatus.IDLE;
            }
            yield return null;
        }
        GoblinAttack();
    }

    public IEnumerator WaitAttack()
    {
        animator.SetBool(Constant.attack, false);
        yield return new WaitForSeconds(waitTime);
        GoblinAttack();
    }
}
