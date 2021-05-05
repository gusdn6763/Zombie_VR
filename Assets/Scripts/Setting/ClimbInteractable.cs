using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClimbInteractable : XRBaseInteractable
{

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (args.interactor is XRDirectInteractor)
            Climber.climbingHand = args.interactor.GetComponent<XRController>();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        if (args.interactor is XRDirectInteractor)
        {
            if (Climber.climbingHand && Climber.climbingHand.name == args.interactor.name)
            {
                Climber.climbingHand = null;
            }
        }
    }


}
