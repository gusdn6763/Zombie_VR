using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagedEffect : MonoBehaviour
{
    //피격 시 사용할 혈흔 효과
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


    //혈흔 효과를 생성하는 함수
    void ShowBloodEffect(Collision coll)
    {
        //총알이 충돌한 지점 산출
        Vector3 pos = coll.contacts[0].point;
        //총알의 충돌했을 때의 법선 벡터
        Vector3 _normal = coll.contacts[0].normal;
        //총알의 충돌 시 방향 벡터의 회전값 계산
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
        //혈흔 효과 생성
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot);
        Destroy(blood, 1.0f);
    }

    public void ShowBloodEffectBullet(Vector3 coll)
    {
        print(coll);
        //총알의 충돌했을 때의 법선 벡터
        Vector3 _normal = coll.normalized;
        //총알의 충돌 시 방향 벡터의 회전값 계산
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
        //혈흔 효과 생성
        GameObject blood = Instantiate<GameObject>(bloodEffect, coll, rot);
        Destroy(blood, 1.0f);
    }
}
