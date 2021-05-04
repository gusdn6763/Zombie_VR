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
    Die,
}

[System.Serializable]
public class EnhanceMob
{
    public float speed;
    public float hp;
    public int damage;

    EnhanceMob(float speed, float hp, int damage)
    {
        this.speed = speed;
        this.hp = hp;
        this.damage = damage;
    }
}


[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class Mob : MonoBehaviour
{
    [Header("쉬움, 보통, 하드순 몬스터 스탯")]
    [SerializeField] private EnhanceMob[] enhanceMob = new EnhanceMob[3];
    [SerializeField] protected float attackDist;

    private Collider monsterCollider;
    private GameObject bloodEffect;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected Transform target;

    private float damping = 1.0f;       //회전할 때의 속도를 조절하는 계수
    protected float hp;
    protected float currentHp;
    protected float speed;
    protected bool isAttack;
    [HideInInspector] public int damage;

    public CharacterStatus enemyStatus = CharacterStatus.IDLE;

    public void Awake()
    {
        monsterCollider = GetComponentInChildren<Collider>();
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

    public void Update()
    {
        if (!(enemyStatus == CharacterStatus.Die) && agent.enabled)
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
    }

    public virtual void StartingMob()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
        EnhanceMob();
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
                case CharacterStatus.Die:
                    break;
            }
        }
    }

    public void Stop()
    {
        if (agent.enabled)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
    }

    public bool isViewPlayer()
    {
        bool isView = false;
        RaycastHit hit;

        //적 캐릭터와 주인공 사이의 방향 벡터를 계산
        Vector3 dir = (target.position - transform.position);
        //레이캐스트를 투사해서 장애물이 있는지 여부를 판단
        if (Physics.Raycast(transform.position, dir, out hit, 2000, 1 << 3))
        {
            isView = (hit.collider.CompareTag(Constant.hitBox));
        }
        return isView;
    }

    //주인공을 추적할 때 이동시키는 함수
    public void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale && agent.enabled)
        {
            agent.destination = pos;
            agent.isStopped = false;
        }
    }

    public void EnhanceMob()
    {
        damage = enhanceMob[GameManager.instance.Difficulty - 1].damage;
        speed = enhanceMob[GameManager.instance.Difficulty - 1].speed;
        hp = enhanceMob[GameManager.instance.Difficulty - 1].hp;
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

    public void ShowBloodEffectBullet(Vector3 coll)
    {
        if (coll == Vector3.zero)
        {
            return;
        }
        //총알의 충돌했을 때의 법선 벡터
        Vector3 _normal = coll.normalized;
        //총알의 충돌 시 방향 벡터의 회전값 계산
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
        //혈흔 효과 생성
        GameObject blood = Instantiate<GameObject>(bloodEffect, coll, rot);
        Destroy(blood, 1.0f);
    }

    public void Die()
    {
        agent.enabled = false;
        enemyStatus = CharacterStatus.Die;
        speed = 0f;
        StopAllCoroutines();
        monsterCollider.enabled = false;
        gameObject.tag = "Untagged";
        isAttack = false;
        print("11");
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

