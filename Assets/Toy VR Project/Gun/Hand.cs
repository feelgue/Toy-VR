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
        
        // 총을 쏘는 로직은 Gun 스크립트의 Update 함수에서 처리되므로,
        // 여기서는 isFire 상태만 업데이트합니다.
        // gun.PullTrigger(isFire); // 이 부분은 이제 필요 없습니다.
        if (!isFire)
        {
            gun.PullTrigger(false);
        }
    }

    private float timeCache;
    void Update()
    {
        if(!isFire){ return;}

        if (Time.time < timeCache + hapticInterval) {return;}
        timeCache = Time.time;
        
        // Raycast가 Enemy에 닿았을 때만 진동을 보내도록
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                SendHaptic();
            }
        }
    }
}