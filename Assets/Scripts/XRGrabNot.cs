using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class XRGrabNot : XRGrabInteractable
{
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
