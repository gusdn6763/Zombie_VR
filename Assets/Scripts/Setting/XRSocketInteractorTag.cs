using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketInteractorTag : XRSocketInteractor
{
    public string targetTag;

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        //1첫 시작 & 여러번 반복
        print("dd");
        return base.CanSelect(interactable) && interactable.CompareTag(targetTag);
    }


    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        //3 한번만 실행?
        print("ad");
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
        //2 한번만 실행
        print("aa");
        base.OnHoverEntering(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        //6
        print("ab");
        base.OnSelectExited(args);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        //5
        print("aaa");
        base.OnSelectExiting(args);
    }

    protected override void OnSelectExited(XRBaseInteractable interactable)
    {
        print("Aaaaa");
        base.OnSelectExited(interactable);
    }
}