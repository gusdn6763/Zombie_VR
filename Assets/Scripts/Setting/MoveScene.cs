using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScene : MonoBehaviour
{
    public int moveStageLevel = 0;
    public Vector3 startPosition;
    public Vector3 startRotation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.player))
        {
            if (Player.instance.keyCheck)
            {
                GameManager.instance.MoveStage(moveStageLevel, startPosition, startRotation);
            }
        }
    }
}
