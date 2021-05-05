using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketInteractorTag : XRSocketInteractor
{
    public string targetTag;
    [SerializeField] private bool realShowInteractableHoverMeshes;


    protected override void Start()
    {
        base.Start();
    }

    protected override void DrawHoveredInteractables()
    {
        if (realShowInteractableHoverMeshes)
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
}