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
    DIE
}

[RequireComponent(typeof(Animator), typeof(NavMeshAgent), typeof(AudioSource))]
public class Mob : MovingObject
{
    [SerializeField] protected float viewRange = 45.0f;
    [SerializeField] protected float attackDist;
    [SerializeField] protected float damage = 2;
    [SerializeField] protected bool isAttack;

    private NavMeshAgent agent;
    private AudioSource threatSound;
    protected Animator animator;
    public Transform target;

    private Vector3 _traceTarget;
    private float damping = 1.0f;       //회전할 때의 속도를 조절하는 계수
    private bool soundPlaying = false;
    public CharacterStatus enemyStatus;

    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = speed;
            //추적 상태의 회전계수
            damping = 7.0f;
            TraceTarget(_traceTarget);
        }
    }

    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        threatSound = GetComponent<AudioSource>();
    }

    public virtual void Start()
    {
        currentHp = HP;
        target = Player.instance.transform;
        agent.autoBraking = false;
        agent.updateRotation = false;
        threatSound.volume = 0f;
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

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }

    IEnumerator CheckState()
    {
        //오브젝트 풀에 생성 시 다른 스크립트의 초기화를 위해 대기
        yield return new WaitForSeconds(1.0f);

        while (!isDie)
        {
            if (enemyStatus == CharacterStatus.DIE)
            {
                yield break;
            }
            float dist = Vector3.Distance(target.position, transform.position);
            if(dist <= 10 && threatSound.clip != null)
            {
                threatSound.volume = 1 - (dist / 10);
                if (!soundPlaying)
                {
                    soundPlaying = true;
                    StartCoroutine(ThreatSound());
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
    IEnumerator ThreatSound()
    {
        threatSound.Play();
        yield return new WaitForSeconds(threatSound.clip.length);
        soundPlaying = false;
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
        while (!isDie)
        {
            yield return (0.3f);
            //상태에 따라 분기 처리
            switch (enemyStatus)
            {
                case CharacterStatus.TRACE:
                    if (isAttack)
                    {
                        isAttack = false;
                        animator.SetBool(Constant.attack, isAttack);
                    }
                    traceTarget = target.position;
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
                case CharacterStatus.DIE:
                    this.gameObject.tag = "Untagged";
                    isDie = true;
                    isAttack = false;
                    Stop();
                    animator.SetTrigger(Constant.die);
                    break;
            }
        }
    }

    public virtual void Attacked()
    {
        isAttack = false;
        animator.SetBool(Constant.attack, isAttack);
    }

    //순찰 및 추적을 정지시키는 함수
    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }

    public override void Die()
    {
        enemyStatus = CharacterStatus.DIE;
        base.Die();
    }
    public void Dead()              //애니메이션에서 실행
    {
        if (threatSound.isPlaying)
        {
            threatSound.Stop();
        }
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Stop();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Stop();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            print("a");
            Stop();
        }
    }
}

