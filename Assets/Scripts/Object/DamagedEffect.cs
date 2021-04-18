using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagedEffect : MonoBehaviour
{
    private const string bulletTag = "BULLET";
    //���� ������
    private float hp = 100.0f;
    //�ʱ� ���� ��ġ
    private float initHp = 100.0f;

    //�ǰ� �� ����� ���� ȿ��
    private GameObject bloodEffect;

    //���� ������ �������� ������ ����
    public GameObject hpBarPrefab;
    //���� �������� ��ġ�� ������ ������
    public Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);
    //�θ� �� Canvas ��ü
    private Canvas uiCanvas;
    //���� ��ġ�� ���� fillAmount �Ӽ��� ������ Image
    private Image hpBarImage;

    void Start()
    {
        //���� ȿ�� �������� �ε�
        bloodEffect = Resources.Load<GameObject>("BulletImpactFleshBigEffect");
        //���� �������� ���� �� �ʱ�ȭ
        SetHpBar();
    }

    void SetHpBar()
    {
        uiCanvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();
        //UI Canvas ������ ���� �������� ����
        GameObject hpBar = Instantiate<GameObject>(hpBarPrefab, uiCanvas.transform);
        //fillAmount �Ӽ��� ������ Image�� ����
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];

        //���� �������� ���󰡾� �� ���� ������ �� ����
        // var _hpBar = hpBar.GetComponent<EnemyHpBar>();
        // _hpBar.targetTr = this.gameObject.transform;
        // _hpBar.offset = hpBarOffset;
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == bulletTag)
        {
            //���� ȿ���� �����ϴ� �Լ� ȣ��
            ShowBloodEffect(coll);
            //�Ѿ� ����
            //Destroy(coll.gameObject);
            coll.gameObject.SetActive(false);

            //���� ������ ����
            //hp -= coll.gameObject.GetComponent<BulletCtrl>().damage;
            //���� �������� fillAmount �Ӽ��� ����
            hpBarImage.fillAmount = hp / initHp;

            if (hp <= 0.0f)
            {
                //�� ĳ������ ���¸� DIE�� ����
                //GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
                //�� ĳ���Ͱ� ����� ���� ���� �������� ���� ó��
                hpBarImage.GetComponentsInParent<Image>()[1].color = Color.clear;
                //Capsule Collider ������Ʈ�� ��Ȱ��ȭ
                GetComponent<CapsuleCollider>().enabled = false;
            }
        }
    }

    //���� ȿ���� �����ϴ� �Լ�
    void ShowBloodEffect(Collision coll)
    {
        //�Ѿ��� �浹�� ���� ����
        Vector3 pos = coll.contacts[0].point;
        //�Ѿ��� �浹���� ���� ���� ����
        Vector3 _normal = coll.contacts[0].normal;
        //�Ѿ��� �浹 �� ���� ������ ȸ���� ���
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
        //���� ȿ�� ����
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot);
        Destroy(blood, 1.0f);
    }
}
