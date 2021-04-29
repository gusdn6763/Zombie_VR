using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseMobAttack : MonoBehaviour
{
    [SerializeField] private MonsterAttack[] monsterAttacks;
    private int damage;

    private void Awake()
    {
        damage = GetComponent<Mob>().damage;
    }

    public void Attacking()
    {
        for (int i = 0; i < monsterAttacks.Length; i++)
        {
            monsterAttacks[i].attacking = true;
            monsterAttacks[i].damage = damage;
        }
    }

    public void Attacked()
    {
        for (int i = 0; i < monsterAttacks.Length; i++)
        {
            monsterAttacks[i].attacking = false;
        }
    }
}
