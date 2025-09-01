using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
public class Hand : MonoBehaviour
{
    public ActionBasedController abc; //캡틱 진동을 주기 위해 컨트롤러 컴포넌트 참조
    public Gun gun; //총에 트리거 효과를 주기 위해 참조 

    
    public InputActionProperty triggerButton;
    private void Awake()
    {
        if (!abc) abc = GetComponent<ActionBasedController>();
        if (!gun) gun = GetComponentInChildren<Gun>();
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

    public bool isFire; //트리거가 눌렸으래 True
    public float hapticInterval; //햅틱 피드백 간격
    [Range(0,1)]
    public float hapticIntensity; //진동 강도
    public float hapticDuration; //진동 지속시간

    public void SendHaptic()
    {
        //컨트롤러에 진동 보내는 함수 
        abc.SendHapticImpulse(hapticIntensity, hapticDuration);
    }
    void OntriggerInput(InputAction.CallbackContext context)
    {
        isFire = context.ReadValueAsButton(); //버튼이 눌렸으면 true ,떄졌으면 false를 반환하는 함수
        
        print($"트리거 눌림 : {isFire}");
        
        gun.PullTrigger(isFire);
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
