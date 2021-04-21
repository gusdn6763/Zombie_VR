using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingObject : MonoBehaviour
{
    public float hp;
    public float currentHp;
    public float speed;

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