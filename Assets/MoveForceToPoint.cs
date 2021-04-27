using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForceToPoint : MonoBehaviour
{
    [SerializeField] private Transform[] points;

    private Mob mobAi;
    private float speed = 1f;
    private int i = 0;

    public void MoveToPoint()
    {
        mobAi.StopAllCoroutines();
        transform.position = Vector3.MoveTowards(transform.position, points[i].transform.position, speed * Time.deltaTime);
    }

    IEnumerator MoveToPointCoroutine()
    {
        yield return null;
    }

}
