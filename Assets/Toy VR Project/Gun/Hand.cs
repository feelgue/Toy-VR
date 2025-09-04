using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
public class Hand : MonoBehaviour
{
    public ActionBasedController abc;
    public Gun gun;
    public XRRayInteractor rayInteractor; // Ray Interactor 참조

    public InputActionProperty triggerButton;
    private void Awake()
    {
        if (!abc) abc = GetComponent<ActionBasedController>();
        if (!gun) gun = GetComponentInChildren<Gun>();
        if (!rayInteractor) rayInteractor = GetComponentInChildren<XRRayInteractor>();

        triggerButton.EnableDirectAction();
    }

    void OnEnable()
    {
        triggerButton.action.performed += OntriggerInput;
    }

    private void OnDisable()
    {
        triggerButton.action.performed -= OntriggerInput;
    }

    private void OnDestroy()
    {
        triggerButton.DisableDirectAction();
    }

    public bool isFire;
    public float hapticInterval;
    [Range(0,1)]
    public float hapticIntensity;
    public float hapticDuration;

    public void SendHaptic()
    {
        abc.SendHapticImpulse(hapticIntensity, hapticDuration);
    }
    void OntriggerInput(InputAction.CallbackContext context)
    {
        isFire = context.ReadValueAsButton();

        print($"트리거 눌림 : {isFire}");
    }

    private float timeCache;
    void Update()
    {
        if(!isFire){ return;}

        if (Time.time < timeCache + hapticInterval) {return;}
        timeCache = Time.time;
        
        SendHaptic();
        
    }
}