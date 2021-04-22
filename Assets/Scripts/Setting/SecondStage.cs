using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondStage : MonoBehaviour
{
    public SecondStage instance;

    [SerializeField] Canvas canvas;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public bool check = false;
    void Start()
    {
        GameManager.instance.StartScene();
        ObjectPoolManager.instance.transform.position = new Vector3(-41f, 2f, 10f);
        ObjectPoolManager.instance.transform.rotation = Quaternion.Euler(0, -85f, 0);
        StartCoroutine(Testing());
    }



    public IEnumerator Testing()
    {
        yield return new WaitUntil( () => GameManager.instance.gameStarting);
        yield return new WaitForSeconds(9f);
        StartCoroutine(PlayerMove(2, new Vector3(0, 0, 1f)));
    }

    public IEnumerator PlayerMove(int speed, Vector3 dir)
    {
        while (true)
        {
            Player.instance.Move(dir, speed);
            yield return null;
        }
    }
}
