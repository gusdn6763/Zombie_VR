using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    [SerializeField] private AttackSite attackSite;

    private void Start()
    {
        attackSite.damage = damage;
    }


}
