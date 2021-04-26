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
}
