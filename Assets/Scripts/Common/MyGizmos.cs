using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public float _radius = 0.1f;

    private void OnDrawGizmos()
    {
        //기즈모 색상 설정
        Gizmos.color = Color.red;
        //구체 모양의 기즈모 생성. 인자는 (생성 위치, 반지름)
        Gizmos.DrawWireSphere(transform.position, _radius); 
    }
}
