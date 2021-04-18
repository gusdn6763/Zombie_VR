using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingObject : MonoBehaviour
{
    public float HP;
    public float currentHp;
    public float speed;
    public bool dmgCheck = true;
    public bool isDie = false;

    public virtual void Awake()
    {

    }

    public virtual void Start()
    {
        currentHp = HP;
    }

    public virtual void Damaged(float damage)
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