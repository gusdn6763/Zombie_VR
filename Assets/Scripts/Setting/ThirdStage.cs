using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdStage : MonoBehaviour
{
    void Start()
    {
        Player.instance.moveImpossible = false;
        GameManager.instance.StartScene();
    }

    public IEnumerator Testing()
    {
        yield return new WaitUntil(() => GameManager.instance.gameStarting);
        yield return new WaitForSeconds(9f);
    }
}
