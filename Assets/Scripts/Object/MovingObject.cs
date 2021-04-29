using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingObject : MonoBehaviour
{
    [SerializeField] protected float hp;
    [SerializeField] protected float currentHp;
    [SerializeField] protected float speed;

    public int damage;

    public virtual void Die()
    {
        speed = 0f;
        StopAllCoroutines();
    }
}