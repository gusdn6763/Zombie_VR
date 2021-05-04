using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoubleGun : Gun
{
    public XRSimpleInteractable secondHandGrabPoints;
    private XRBaseInteractor secondInteractor;
    private Quaternion attachInitialRotation;
    public enum TwoHandRotationType { None, First, Second };
    public TwoHandRotationType twoHandRotationType;
    private Quaternion initialRotationOffset;

    //시작시 2번째 그랩 위치의 
    void Start()
    {
        secondHandGrabPoints.onSelectEntered.AddListener(OnSecondHandGrab);
        secondHandGrabPoints.onSelectExited.AddListener(OnSecondHandRelease);
    }

    //selectingInteractor는 레이
    //secondInteractor는 2번째 그랩
    //interactor는 플레이어 손 오브젝트
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (secondInteractor && selectingInteractor)
        {
            selectingInteractor.attachTransform.rotation = GetTwoHandRotation();
        }
        base.ProcessInteractable(updatePhase);
    }

    private Quaternion GetTwoHandRotation()
    {
        Quaternion targetRotation;
        targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position);
        return targetRotation;
    }


    public void OnSecondHandGrab(XRBaseInteractor interactor)
    {
        if (interactor != null)
        {
            secondInteractor = interactor;
            if (selectingInteractor)
            {
                initialRotationOffset = Quaternion.Inverse(GetTwoHandRotation()) * selectingInteractor.attachTransform.rotation;
            }
        }
    }

    public void OnSecondHandRelease(XRBaseInteractor interactor)
    {
        Debug.Log("SECOND HAND RELEASE");
        secondInteractor = null;
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        Debug.Log("First Grab Enter");
        base.OnSelectEntered(interactor);
        attachInitialRotation = interactor.attachTransform.localRotation;
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        Debug.Log("First Grab Exit");
        base.OnSelectExited(interactor);
        secondInteractor = null;
        interactor.attachTransform.localRotation = attachInitialRotation;
    }

    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        bool isalreadygrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
        return base.IsSelectableBy(interactor) && !isalreadygrabbed;
    }
}
