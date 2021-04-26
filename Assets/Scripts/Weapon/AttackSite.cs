using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSite : MonoBehaviour
{
    private CapsuleCollider capsuleCollider;
    private bool attackCheck = false;
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (!attackCheck)
        {
            if (other.CompareTag(Constant.zombiePart))
            {
                other.GetComponent<Part>().Damaged(damage, transform.position);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        attackCheck = false;
    }
}
