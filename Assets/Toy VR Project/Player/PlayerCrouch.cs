using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class PlayerCrouch : MonoBehaviour
{
    public Transform cameraOffset; // 인스펙터에 XR Origin > Camera Offset을 연결
    public InputActionProperty crouchAction; // A 버튼 Input Action을 연결

    private float startY;
    private bool isCrouched = false;

    private void Awake()
    {
        // 시작 시 Camera Offset의 Y 위치를 저장
        if (cameraOffset != null)
        {
            startY = cameraOffset.localPosition.y;
        }
        crouchAction.EnableDirectAction();
    }

    private void OnEnable()
    {
        // A 버튼 액션에 함수 연결
        crouchAction.action.performed += OnCrouchAction;
        Debug.Log("버튼을 눌렀습니다");
    }

    private void OnDisable()
    {
        crouchAction.action.performed -= OnCrouchAction;
        Debug.Log("버튼을 땠습니다");
    }

    private void OnDestroy()
    {
        crouchAction.DisableDirectAction();
    }

    private void OnCrouchAction(InputAction.CallbackContext context)
    {
        // 웅크리기 상태를 토글
        isCrouched = !isCrouched;
        
        // 웅크린 상태에 따라 Camera Offset의 Y 위치를 변경
        Vector3 newPosition = cameraOffset.localPosition;
        if (isCrouched)
        {
            newPosition.y = startY - 0.2f; // 0.2m 낮추기
            Debug.Log("움크리기");
        }
        else
        {
            newPosition.y = startY;
            Debug.Log("원상복구");
        }
        
        cameraOffset.localPosition = newPosition;
    }
}