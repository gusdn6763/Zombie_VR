using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerRayScript : MonoBehaviour
{
    private XRInteractorLineVisual xRInteractorLineVisual;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    [Range(0, 1)]
    [SerializeField] private float alphaValue;
    [SerializeField] private float speed = 1f;

    private Material material;
    private Gradient disableGradient = new Gradient();
    private Gradient enableGradient = new Gradient();

    private void Awake()
    {
        xRInteractorLineVisual = GetComponent<XRInteractorLineVisual>();
        material = GetComponent<LineRenderer>().materials[0];
    }

    private void Start()
    {
        enableGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(startColor, 0.0f), new GradientColorKey(endColor, alphaValue) },
            new GradientAlphaKey[] { new GradientAlphaKey(alphaValue, 0.0f), new GradientAlphaKey(alphaValue, alphaValue) }
        );

        disableGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(startColor, 0.0f), new GradientColorKey(endColor, 0.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(0.0f, 0.0f) }
        );
        GradientCheck(true);
    }
    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset = (new Vector2(Time.realtimeSinceStartup * speed, 0f));
    }

    public void GradientCheck(bool check)
    {
        if (check)
        {
            xRInteractorLineVisual.invalidColorGradient = enableGradient;
        }
        else
        {
            xRInteractorLineVisual.invalidColorGradient = disableGradient;
        }
    }
}
