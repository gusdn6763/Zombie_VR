using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdStage : MonoBehaviour
{
    [SerializeField] private Collider[] goblinCollider;
    void Start()
    {
        Player.instance.moveImpossible = false;
        GameManager.instance.StartScene();
    }

    public IEnumerator Testing()
    {
        yield return new WaitUntil(() => GameManager.instance.gameStarting);
        yield return new WaitForSeconds(9f);
        for(int i = 0; i < goblinCollider.Length; i++)
        {
            goblinCollider[i].enabled = true;
        }
    }
}
