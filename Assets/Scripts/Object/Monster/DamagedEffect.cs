using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagedEffect : MonoBehaviour
{
    //�ǰ� �� ����� ���� ȿ��
    private GameObject bloodEffect;

    void Start()
    {
        bloodEffect = Resources.Load<GameObject>("BulletImpactFleshBigEffect");
    }


    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag(Constant.weapon))
    //    {
    //        ShowBloodEffect(collision);
    //    }
    //}


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

    public void ShowBloodEffectBullet(Vector3 coll)
    {
        print(coll);
        //�Ѿ��� �浹���� ���� ���� ����
        Vector3 _normal = coll.normalized;
        //�Ѿ��� �浹 �� ���� ������ ȸ���� ���
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
        //���� ȿ�� ����
        GameObject blood = Instantiate<GameObject>(bloodEffect, coll, rot);
        Destroy(blood, 1.0f);
    }
}
