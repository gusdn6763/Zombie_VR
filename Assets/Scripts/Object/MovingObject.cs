using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingObject : MonoBehaviour
{
    public float HP;
    public float currentHp;
    public float speed = 0f;
    public bool dmgCheck = true;
    public bool isDie = false;

    public virtual void Awake()
    {

    }

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
        Debug.Log(this.gameObject.name+"죽음");
    }
}