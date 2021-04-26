﻿using System;
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

[RequireComponent(typeof(Animator), typeof(NavMeshAgent), typeof(AudioSource))]
public class Mob : MovingObject
{
    [SerializeField] private MonsterAttack[] monsterAttacks;
    [SerializeField] protected float viewRange = 45.0f;
    [SerializeField] protected float attackDist;
    [SerializeField] protected int damage;
    [SerializeField] protected bool isAttack;

    private NavMeshAgent agent;
    private AudioSource threatSound;
    protected Animator animator;
    protected Transform target;

    private float damping = 1.0f;       //회전할 때의 속도를 조절하는 계수
    private bool startingMob = false;
    [SerializeField] protected CharacterStatus enemyStatus = CharacterStatus.IDLE;

    public virtual void Awake()
    {
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
        threatSound.Play();
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
        if (SoundManager.instance.soundIsOn)
        {
            threatSound.mute = false;
            threatSound.volume = SoundManager.instance.currentSoundVolume;
        }
        else
        {
            threatSound.mute = true;
        }
    }

    IEnumerator CheckState()
    {
        startingMob = true;
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
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
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

    public bool isViewPlayer()
    {
        bool isView = false;
        RaycastHit hit;

        //적 캐릭터와 주인공 사이의 방향 벡터를 계산
        Vector3 dir = (target.position - transform.position).normalized;

        //레이캐스트를 투사해서 장애물이 있는지 여부를 판단
        if (Physics.Raycast(transform.position, dir, out hit, viewRange,1 << 3))
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

    public void Attacking()
    {
        for(int i = 0; i < monsterAttacks.Length; i++)
        {
            monsterAttacks[i].attacking = true;
            monsterAttacks[i].damage = damage;
        }
    }

    public void Attacked()
    {
        for (int i = 0; i < monsterAttacks.Length; i++)
        {
            monsterAttacks[i].attacking = false;
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
