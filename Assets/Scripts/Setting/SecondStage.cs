using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondStage : MonoBehaviour
{
    [SerializeField] private Transform movePoint;
    private Transform[] movePoints;

    public float moveSpeed;


    void Awake()
    {
        movePoints = movePoint.GetComponentsInChildren<Transform>();
    }

    void Start()
    {
        Player.instance.moveImpossible = true;
        GameManager.instance.StartScene();
        StartCoroutine(Testing());
    }



    public IEnumerator Testing()
    {
        yield return new WaitUntil( () => GameManager.instance.gameStarting);
        yield return new WaitForSeconds(10f);
        StartCoroutine(PlayerMove());
    }

    public IEnumerator PlayerMove()
    {
        int i = 1;
        while (!GameManager.instance.isGameOver)
        {
            Player.instance.transform.position = Vector3.MoveTowards(Player.instance.transform.position, movePoints[i].position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(Player.instance.transform.position, movePoints[i].position) <= 1f)
            {
                i++;
            }
            yield return null;
        }
    }
}
