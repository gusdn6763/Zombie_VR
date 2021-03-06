using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    [SerializeField] private Gun gun;
    [SerializeField] private DoubleGun doubleGun;
    [SerializeField] private Transform bulletBox;

    public List<BulletCtrl> bulletManager = new List<BulletCtrl>();
    public BulletCtrl bullet;
    public int bulletCount = 30;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        InstanceBullet(bulletCount);
    }

    /// <summary>
    /// 총알 생성 함수
    /// </summary>
    /// <param name="bulletCount">생성할 총알 갯수</param>
    public void InstanceBullet(float bulletCount = 0)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            BulletCtrl bulletTmp = Instantiate(bullet, bulletBox);
            bulletManager.Add(bulletTmp);
            bulletTmp.transform.name = "Bullet" + i.ToString();
            bulletTmp.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 디버깅
    /// </summary>
    public void MakeGun()
    {
        Instantiate(gun, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.rotation);
    }

    public void MakeDoubleGun()
    {
        Instantiate(doubleGun, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.rotation);
    }
}