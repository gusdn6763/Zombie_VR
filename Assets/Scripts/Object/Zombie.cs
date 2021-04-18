using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Zombie_Parts
{
    public string partName;
    public BoxCollider collObject;
    public GameObject brokeObject;
}
public class Zombie : Mob
{
    public Action<int> test;
    [SerializeField] private Zombie_Parts[] zombie_Parts;


}
