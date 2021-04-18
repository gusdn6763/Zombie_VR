using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class Mob : MovingObject
{
    protected Animator animator;
    //적 캐릭터의 추적 사정 거리의 범위
    public float viewRange = 15.0f;
    [Range(0, 360)]
    //적 캐릭터의 시야각
    public float viewAngle = 120.0f;
    private int layerMask;
    //NavMeshAgent 컴포넌트를 저장할 변수
    private NavMeshAgent agent;
    public Transform target;

    private readonly float traceSpeed = 4.0f;
    //회전할 때의 속도를 조절하는 계수
    private float damping = 1.0f;

    //추적 대상의 위치를 저장하는 변수
    private Vector3 _traceTarget;

    public float attackDist;
    //traceTarget 프로퍼티 정의(getter,setter)

    public bool isAttack;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            //추적 상태의 회전계수
            damping = 7.0f;
            TraceTarget(_traceTarget);
        }
    }

    public enum CharacterStatus
    {
        IDLE,
        MOVE,
        TRACE,
        ATTACK,
        DIE
    }

    public CharacterStatus enemyStatus;

    //애니메이터 컨트롤러에 정의한 파라미터의 해시값을 미리 추출
    private readonly int hashMove = Animator.StringToHash("Run");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");
    private readonly int hashOffset = Animator.StringToHash("Offset");
    private readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");


    //NavMeshAgent의 이동 속도에 대한 프로퍼티 정의(getter)
    public float speed
    {
        get { return agent.velocity.magnitude; }
    }
    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = Player.instance.transform;
    }

    public override void Start()
    {
        base.Start();
        agent.autoBraking = false;
        agent.updateRotation = false;
    }


    void Update()
    {
        //적 캐릭터가 이동 중일 때만 회전
        if (agent.isStopped == false)
        {
            //NavMeshAgent가 가야 할 방향 벡터를 쿼터니언 타입의 각도로 변환
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            //보간 함수를 사용해 점진적으로 회전시킴
            this.transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * damping);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }


    //주인공을 추적할 때 이동시키는 함수
    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;
    }

    //순찰 및 추적을 정지시키는 함수
    public void Stop()
    {
        agent.isStopped = true;
        //바로 정지하기 위해 속도를 0으로 설정
        agent.velocity = Vector3.zero;
    }


    IEnumerator CheckState()
    {
        //오브젝트 풀에 생성 시 다른 스크립트의 초기화를 위해 대기
        yield return new WaitForSeconds(1.0f);

        //적 캐릭터가 사망하기 전까지 도는 무한루프
        while (!isDie)
        {
            //상태가 사망이면 코루틴 함수를 종료시킴
            if (enemyStatus == CharacterStatus.DIE) yield break;
            //주인공과 적 캐릭터 간의 거리를 계산
            float dist = Vector3.Distance(target.position, transform.position);
            //공격 사정거리 이내의 경우
            if (dist <= attackDist)
            {
                //주인공과의 거리에 장애물 여부를 판단
                if (isViewPlayer())
                    enemyStatus = CharacterStatus.ATTACK;   //장애물이 없으면 공격 모
            }
            else
            {
                enemyStatus = CharacterStatus.TRACE;    //장애물이 있으면 추적 모드
            }
            //0.3초 동안 대기하는 동안 제어권을 양보
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
        if (Physics.Raycast(transform.position, dir, out hit, viewRange, layerMask))
        {
            isView = (hit.collider.CompareTag("PLAYER"));
        }
        return isView;
    }

    //상태에 따라 적 캐릭터의 행동을 처리하는 코루틴 함수
    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return (0.3f);
            //상태에 따라 분기 처리
            switch (enemyStatus)
            {
                case CharacterStatus.TRACE:
                    //총 발사 정지
                    isAttack = false;
                    //주인공의 위치를 넘겨 추적 모드로 변경
                    traceTarget = target.position;
                    animator.SetBool(hashMove, true);
                    break;
                case CharacterStatus.ATTACK:
                    //순찰 및 추적을 정지
                    Stop();
                    animator.SetBool(hashMove, false);
                    //총알 발사 시작
                    if (isAttack == false)
                        isAttack = true;
                    break;
                case CharacterStatus.DIE:
                    this.gameObject.tag = "Untagged";

                    isDie = true;
                    isAttack = false;
                    //순찰 및 추적을 정지
                    Stop();
                    //사망 애니메이션의 종류를 지정
                    //animator.SetInteger(hashDieIdx, UnityEngine.Random.Range(0, 3));
                    //사망 애니메이션 실행
                    animator.SetTrigger(hashDie);
                    //Capsule Collider 컴포넌트를 비활성화
                    GetComponent<CapsuleCollider>().enabled = false;
                    break;
            }
        }
    }

    public override void Damaged(float damage)
    {
        base.Damaged(damage);
    }

    public override void Die()
    {
        enemyStatus = CharacterStatus.DIE;
        float percent = UnityEngine.Random.Range(0, 101);
        base.Die();
    }
    public void Dead()              //애니메이션에서 실행
    {
        Destroy(this.gameObject);
    }

}

