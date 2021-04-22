using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDoor : MonoBehaviour
{
    [SerializeField] private float waitTime;
    [SerializeField] private float moveTime;
    [SerializeField] private Vector3 dir;


    private void Start()
    {
        MoveStartDoor();
    }
    public void MoveStartDoor()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            if (GameManager.instance.gameStarting)
            {
                yield return new WaitForSeconds(9f);
                while (moveTime >= 0)
                {
                    moveTime -= Time.deltaTime;
                    gameObject.transform.Translate(dir * Time.deltaTime);
                    yield return null;
                }
                StopAllCoroutines();
            }
            yield return new WaitForSeconds(1f);
        }
    }

}
