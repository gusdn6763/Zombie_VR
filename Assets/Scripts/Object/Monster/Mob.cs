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

[RequireComponent(typeof(Animator), typeof(NavMeshAgent), typeof(Rigidbody))]
public class Mob : MovingObject
{
    [SerializeField] protected float viewRange = 45.0f;
    [SerializeField] protected float attackDist;
    [SerializeField] protected int damage;
    [SerializeField] protected bool isAttack;

    private Rigidbody rigi;
    private NavMeshAgent agent;
    private AudioSource threatSound;
    protected Animator animator;
    protected Transform target;

    private float damping = 1.0f;       //회전할 때의 속도를 조절하는 계수
    private bool soundPlaying = false;
    private bool startingMob = false;
    public CharacterStatus enemyStatus = CharacterStatus.IDLE;

    public virtual void Awake()
    {
        rigi = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        threatSound = GetComponent<AudioSource>();
    }

    public virtual void Start()
    {
        target = Player.instance.transform;
        currentHp = hp;
        agent.autoBraking = false;
        agent.updateRotation = false;
    }

    public virtual void Update()
    {
        //적 캐릭터가 이동 중일 때만 회전
        if (agent.isStopped == false && startingMob)
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

    public void StartingMob()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }

    IEnumerator CheckState()
    {
        startingMob = true;
        yield return new WaitForSeconds(1.0f);

        while (true)
        {
            float dist = Vector3.Distance(target.position, transform.position);
            if(dist <= 10 && threatSound.clip != null)
            {
                threatSound.volume = 1 - ((dist / 10) * SoundManager.instance.audioSourceEffects[0].volume);
                if (!soundPlaying)
                {
                    threatSound.Play();
                    soundPlaying = true;
                    yield return new WaitForSeconds(threatSound.clip.length);
                    soundPlaying = false;
                }
            }
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

    public bool isViewPlayer()
    {
        bool isView = false;
        RaycastHit hit;

        //적 캐릭터와 주인공 사이의 방향 벡터를 계산
        Vector3 dir = (target.position - transform.position).normalized;

        //레이캐스트를 투사해서 장애물이 있는지 여부를 판단
        if (Physics.Raycast(transform.position, dir, out hit, viewRange))
        {
            isView = (hit.collider.CompareTag(Constant.player));
        }
        return isView;
    }

    //주인공을 추적할 때 이동시키는 함수
    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;
    }

    IEnumerator Action()
    {
        while (true)
        {
            yield return (0.3f);
            switch (enemyStatus)
            {
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

    /// <summary>
    /// 순찰 및 추적을 정지시키는 함수
    /// </summary>
    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }

    public override void Die()
    {
        base.Die();
        gameObject.tag = "Untagged";
        isAttack = false;
        Stop();
        animator.SetTrigger(Constant.die);
    }

    /// <summary>
    /// 애니메이션에서 실행
    /// </summary>
    public virtual void Dead()              
    {
        Destroy(this.gameObject, 1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isAttack && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Damaged(damage);
        }
    }
}

