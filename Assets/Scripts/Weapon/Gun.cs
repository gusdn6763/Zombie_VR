using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class Gun : Weapon
{
    [Header("총")]
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private int maxBullet = 10;
    [SerializeField] private int remainingBullet = 10;
    [SerializeField] private float reloadTime = 2.0f;
    [SerializeField] private float delayTime = 0f;
    [SerializeField] private bool isReloading = false;

    private ParticleSystem muzzleFlash;
    protected Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        muzzleFlash = barrelLocation.GetComponentInChildren<ParticleSystem>();
        interactionManager = FindObjectOfType<XRInteractionManager>();
    }

    public void Attack()
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
            UpdateBulletText();
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
        SoundManager.instance.PlaySE(Constant.reloading);
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        remainingBullet = maxBullet;
        UpdateBulletText();
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
        SoundManager.instance.PlaySE(Constant.shoot);
        bullet.barrelLocation = barrelLocation;
        bullet.damage = damage;
        bullet.gameObject.SetActive(true);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        if (grapingHand == HandState.LEFT)
        {
            Player.instance.playerUi.UIReflectionlLeftBullet(0, 0);
        }
        else if (grapingHand == HandState.RIGHT)
        {
            Player.instance.playerUi.UIReflectionlRightBullet(0, 0);
        }
        base.OnSelectExiting(args);
    }

    public void UpdateBulletText()
    {
        if (grapingHand == HandState.LEFT)
        {
            Player.instance.playerUi.UIReflectionlLeftBullet(remainingBullet, maxBullet);
        }
        else if (grapingHand == HandState.RIGHT)
        {
            Player.instance.playerUi.UIReflectionlRightBullet(remainingBullet, maxBullet);
        }
    }
}
