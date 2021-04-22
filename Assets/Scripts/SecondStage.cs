using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondStage : MonoBehaviour
{
    public bool check = false;
    void Start()
    {
        GameManager.instance.StartScene();
        StartCoroutine(Testing());
        ObjectPoolManager.instance.transform.position = new Vector3(-40.25f, 2.5f, 10f);
        ObjectPoolManager.instance.transform.rotation = Quaternion.Euler(0, 0, 0);
        ObjectPoolManager.instance.MakeGun();
    }

    void Update()
    {
        if (check)
        {
            Player.instance.Move(new Vector3(0, 0, 1f), 1);
        }
    }

    public IEnumerator Testing()
    {
        yield return new WaitForSeconds(9f);
        check = true;
    }

}
