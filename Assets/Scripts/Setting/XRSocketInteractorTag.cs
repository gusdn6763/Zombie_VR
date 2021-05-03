using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketInteractorTag : XRSocketInteractor
{
    public string targetTag;

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        //1ù ���� & ������ �ݺ�
        return base.CanSelect(interactable) && interactable.CompareTag(targetTag);
    }


    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        //3 �ѹ��� ����?
        if (args.interactable.CompareTag(targetTag))
        {

            showInteractableHoverMeshes = true;
            base.OnHoverEntered(args);
        }
        else
        {
            showInteractableHoverMeshes = false;
        }
    }
    protected override void OnHoverEntering(HoverEnterEventArgs args)
    {
        //2 �ѹ��� ����
        base.OnHoverEntering(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        //6
        base.OnSelectExited(args);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        //5
        base.OnSelectExiting(args);
    }

    protected override void OnSelectExited(XRBaseInteractable interactable)
    {
        base.OnSelectExited(interactable);
    }
}