using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketInteractorTag : XRSocketInteractor
{
    public string targetTag;
    private bool isUsing = false;

    protected override void DrawHoveredInteractables()
    {
        if (!isUsing)
        {
            base.DrawHoveredInteractables();
        }
    }

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        //1ù ���� & ������ �ݺ�
        return base.CanSelect(interactable) && interactable.CompareTag(targetTag);
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (args.interactable.CompareTag(targetTag))
        {
            if (!isUsing)
            {
                GetComponent<MeshRenderer>().enabled = true;
            }
        }
        base.OnHoverEntered(args);
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        GetComponent<MeshRenderer>().enabled = false;
        base.OnHoverExited(args);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactable.CompareTag(targetTag))
        {
            isUsing = true;
        }
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        if (args.interactable.CompareTag(targetTag))
        {
            isUsing = false;
        }
        base.OnSelectExited(args);
    }
}