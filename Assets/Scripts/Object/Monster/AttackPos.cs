using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPos : MonoBehaviour
{
    private CapsuleCollider cap;
    public int damage;

    private void Awake()
    {
        cap = GetComponent<CapsuleCollider>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject);
        if (collision.gameObject.CompareTag(Constant.player))
        {
            collision.gameObject.GetComponent<Player>().Damaged(damage);
        }
    }
}
