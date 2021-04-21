using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class Gun : Weapon
{
    [Header("총")]
    [SerializeField] private Transform barrelLocation;

    private ParticleSystem muzzleFlash;
    protected Animator animator;
    public GameObject muzzleFlashPrefab;

    //최대 총알 수
    public int maxBullet = 10;
    //남은 총알 수
    public int remainingBullet = 10;
    //재장전 시간
    public float reloadTime = 2.0f;
    //총 발사 딜레이
    public float delayTime = 0f;
    //총 발사 여부
    //재장전 여부를 판단할 변수
    private bool isReloading = false;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        muzzleFlash = barrelLocation.GetComponentInChildren<ParticleSystem>();
    }

    public override void Attack()
    {
        if (!attackCheck)
        {
            attackCheck = true;
            StartCoroutine(AttackCoroutine());
        }
    }
    IEnumerator AttackCoroutine()
    {
        if (!isReloading)
        {
            --remainingBullet;
            animator.SetTrigger(Constant.fire);

            if (remainingBullet == 0)
            {
                StartCoroutine(Reloading());
            }
        }
        yield return new WaitForSeconds(delayTime);
        attackCheck = false;
    }
    IEnumerator Reloading()
    {
        SoundManager.instance.PlaySE("Reloading");
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        remainingBullet = maxBullet;
        //UpdateBulletText();
    }

    /// <summary>
    /// 총 발사
    /// </summary>
    public void Fire()
    {
        int Count = ObjectPoolManager.instance.bulletManager.Count;
        for (int i = 0; i < Count; i++)
        {
            if (ObjectPoolManager.instance.bulletManager[i].gameObject.activeSelf)
            {
                if (i == Count - 1)
                {
                    ObjectPoolManager.instance.InstanceBullet(1);
                    BulletActive(ObjectPoolManager.instance.bulletManager[i + 1]);
                    break;
                }
                continue;
            }
            BulletActive(ObjectPoolManager.instance.bulletManager[i]);
            break;
        }
    }

    public void BulletActive(BulletCtrl bullet)
    {
        muzzleFlash.Play();
        SoundManager.instance.PlaySE("Shoot");
        bullet.barrelLocation = barrelLocation;
        bullet.damage = damage;
        bullet.gameObject.SetActive(true);
    }
}
