using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : XRGrabInteractable
{
    private CustomController customController;
    [Header("무기정보")]
    public Vector3 leftAttachPos;
    public Vector3 leftAttachrotation;
    public Vector3 rightAttachPos;
    public Vector3 rightAttachrotation;
    protected bool attackCheck = false;
    public int damage = 0;


    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        print(args.interactor);
        if (this.CompareTag(Constant.weapon) && (args.interactor.CompareTag(Constant.handLeft)
            || args.interactor.CompareTag(Constant.handRight)))
        {
            print("a");
            args.interactor.GetComponentInChildren<CustomController>().GetWeapon(this);
        }
        if (args.interactor.CompareTag(Constant.handRight))
        {
            attachTransform.localPosition = rightAttachPos;
            attachTransform.localRotation = Quaternion.Euler(rightAttachrotation.x, rightAttachrotation.y, rightAttachrotation.z);
        }
        else if (args.interactor.CompareTag(Constant.handLeft))
        {
            attachTransform.localPosition = leftAttachPos;
            attachTransform.localRotation = Quaternion.Euler(leftAttachrotation.x, leftAttachrotation.y, leftAttachrotation.z);
        }
        base.OnSelectEntering(args);
    }
}
