using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    public bool attacking = false;
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (attacking)
        {
            print(other.gameObject);
            if (other.CompareTag(Constant.hitBox))
            {
                other.GetComponentInParent<Player>().Damaged(damage);
            }
        }
    }
}
