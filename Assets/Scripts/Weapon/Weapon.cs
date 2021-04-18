using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : XRGrabInteractable
{
    private CustomController customController;
    [Header("무기정보")]
    public Vector3 leftAttachPos;
    public Vector3 rightAttachPos;
    public float damage = 0f;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        args.interactor.GetComponentInChildren<CustomController>().GetWeapon(this);
        if (args.interactor.CompareTag(Constant.handRight))
        {
            attachTransform.localPosition = rightAttachPos;
        }
        else if (args.interactor.CompareTag(Constant.handLeft))
        {
            attachTransform.localPosition = leftAttachPos;
        }
        base.OnSelectEntering(args);
    }

    public virtual void Attack()
    {

    }
}
