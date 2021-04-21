using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSite : MonoBehaviour
{
    private CapsuleCollider capsuleCollider;
    private bool attackCheck = false;
    public int damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (!attackCheck)
        {
            attackCheck = true;
            if (collision.gameObject.CompareTag(Constant.monster))
            {
                collision.gameObject.GetComponent<MovingObject>().Damaged(damage);
            }
            else if (collision.gameObject.CompareTag(Constant.zombiePart))
            {
                collision.gameObject.GetComponent<Part>().Damaged(damage);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        attackCheck = false;
    }
}
