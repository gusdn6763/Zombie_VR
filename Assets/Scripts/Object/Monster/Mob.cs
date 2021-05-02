using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CharacterStatus
{
    IDLE,
    TRACE,
    ATTACK,
}

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class Mob : MovingObject
{
    [SerializeField] protected float attackDist;

    private GameObject bloodEffect;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected Transform target;

    private float damping = 1.0f;       //회전할 때의 속도를 조절하는 계수
    protected bool isAttack;
    public CharacterStatus enemyStatus = CharacterStatus.IDLE;

    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public virtual void Start()
    {
        target = Player.instance.transform;
        currentHp = hp;
        agent.autoBraking = false;
        agent.updateRotation = false;
        bloodEffect = Resources.Load<GameObject>("BulletImpactFleshBigEffect");
    }

    public virtual void Update()
    {
        //적 캐릭터가 이동 중일 때만 회전
        if (agent.isStopped == false)
        {
            if (agent.desiredVelocity != Vector3.zero)
            {
                //NavMeshAgent가 가야 할 방향 벡터를 쿼터니언 타입의 각도로 변환
                Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);

                //보간 함수를 사용해 점진적으로 회전시킴
                this.transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * damping);
            }
        }
    }

    public void ShowBloodEffectBullet(Vector3 coll)
    {
        if (coll == Vector3.zero)
        {
            return ;
        }
        //총알의 충돌했을 때의 법선 벡터
        Vector3 _normal = coll.normalized;
        //총알의 충돌 시 방향 벡터의 회전값 계산
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
        //혈흔 효과 생성
        GameObject blood = Instantiate<GameObject>(bloodEffect, coll, rot);
        Destroy(blood, 1.0f);
    }

    public virtual void StartingMob()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }

    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(1.0f);

        while (true)
        {
            float dist = Vector3.Distance(target.position, transform.position);
            if (dist <= attackDist && isViewPlayer())
            {
                enemyStatus = CharacterStatus.ATTACK;
            }
            else
            {
                enemyStatus = CharacterStatus.TRACE;
            }
            yield return (0.3f);
        }
    }
    IEnumerator Action()
    {
        while (true)
        {
            yield return (0.3f);
            switch (enemyStatus)
            {
                case CharacterStatus.IDLE:
                    agent.isStopped = true;
                    break;
                case CharacterStatus.TRACE:
                    if (isAttack)
                    {
                        isAttack = false;
                        animator.SetBool(Constant.attack, isAttack);
                    }
                    agent.speed = speed;
                    damping = 7.0f;
                    TraceTarget(target.position);
                    animator.SetBool(Constant.move, true);
                    animator.SetFloat(Constant.speed, speed);
                    break;
                case CharacterStatus.ATTACK:
                    Stop();
                    animator.SetBool(Constant.move, false);
                    if (isAttack == false)
                    {
                        isAttack = true;
                        animator.SetBool(Constant.attack, isAttack);
                    }
                    break;
            }
        }
    }

    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }

    public bool isViewPlayer()
    {
        bool isView = false;
        RaycastHit hit;

        //적 캐릭터와 주인공 사이의 방향 벡터를 계산
        Vector3 dir = (target.position - transform.position);
        //레이캐스트를 투사해서 장애물이 있는지 여부를 판단
        if (Physics.Raycast(transform.position, dir, out hit,10000, 1 << 3))
        {
            isView = (hit.collider.CompareTag(Constant.hitBox));
        }
        return isView;
    }

    //주인공을 추적할 때 이동시키는 함수
    public void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;
    }

    public virtual void EnhanceMob()
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

    public virtual void Damaged(int damage, Vector3 position)
    {
        currentHp -= damage;
        ShowBloodEffectBullet(position);
        if (currentHp <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        base.Die();
        gameObject.tag = "Untagged";
        isAttack = false;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        animator.SetTrigger(Constant.die);
    }

    /// <summary>
    /// 애니메이션에서 실행
    /// </summary>
    public void Dead()
    {
        Destroy(this.gameObject, 1f);
    }

}

