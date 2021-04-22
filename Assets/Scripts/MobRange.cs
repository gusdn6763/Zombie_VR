using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobRange : MonoBehaviour
{
    private Mob[] mobs;
    private bool check = false;

    private void Awake()
    {
        mobs = GetComponentsInChildren<Mob>();
    }

    public void TraceingPlayer()
    {
        for(int i = 0; i < mobs.Length; i++)
        {
            mobs[i].StartingMob();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag(Constant.weapon) || other.CompareTag(Constant.player)) &&
            check == false)
        {
            check = true;
            TraceingPlayer();
        }
    }

}
