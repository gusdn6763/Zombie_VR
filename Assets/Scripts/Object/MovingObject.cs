using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingObject : MonoBehaviour
{
    [SerializeField] protected float hp;
    [SerializeField] protected float currentHp;
    [SerializeField] protected float speed;

    public virtual void Damaged(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        speed = 0f;
        StopAllCoroutines();
    }
}