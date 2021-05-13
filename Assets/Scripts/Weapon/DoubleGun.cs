using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// 두손으로 집을 수 있도록 할시 소켓에 넣으면 실행되지 않는 버그 해결 =>
/// IsSelectableBy에서 isalreadygrabbed값이 true가 되어 상호작용이 막힘 & ProcessInteractable함수로 인해 로테이션값만 바뀜
/// </summary>
public class DoubleGun : Gun
{
    [SerializeField] private XRSimpleInteractable secondHandGrabPoints;
    private XRBaseInteractor secondInteractor;
    private Quaternion attachInitialRotation;
    private Quaternion initialRotationOffset;

    //시작시 2번째 그랩 위치에 삽입
    void Start()
    {
        secondHandGrabPoints.selectEntered.AddListener(OnSecondHandGrab);
        secondHandGrabPoints.selectExited.AddListener(OnSecondHandRelease);
    }


    //selectingInteractor는 레이
    //secondInteractor는 2번째 그랩
    //interactor는 플레이어 손 오브젝트
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (secondInteractor && selectingInteractor && !isInHolster)
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


    public void OnSecondHandGrab(SelectEnterEventArgs args)
    {
        if (args.interactor != null)
        {
            secondInteractor = args.interactor;
            if (selectingInteractor)
            {
                gunRay.GradientCheck(rayCheck);
                initialRotationOffset = Quaternion.Inverse(GetTwoHandRotation()) * selectingInteractor.attachTransform.rotation;
            }
        }
    }

    public void OnSecondHandRelease(SelectExitEventArgs args)
    {
        Debug.Log("SECOND HAND RELEASE");
        gunRay.GradientCheck(false);
        secondInteractor = null;
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        Debug.Log("First Grab Enter");
        base.OnSelectEntering(args);
        gunRay.GradientCheck(false);
        attachInitialRotation = args.interactor.attachTransform.localRotation;
    }


    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        Debug.Log("First Grab Exit");
        base.OnSelectExiting(args);
        secondInteractor = null;
        args.interactor.attachTransform.localRotation = attachInitialRotation;
    }

    ////첫번째로 집을때 selectingInteractor는 널값
    ////interactor는 컨트롤러값
    ////반환 실행

    ////두번째로 집을때 selectingInteractor는 레이값
    ////interactor는 컨트롤러값 반환 실행x => 상호작용 하지않는다 => secondHandGrabPoints함수쪽 실행
    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        if (isInHolster)
        {
            return base.IsSelectableBy(interactor);
        }
        else
        {
            bool isalreadygrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
            return base.IsSelectableBy(interactor) && !isalreadygrabbed;
        }
    }
}
