using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class Gun : MonoBehaviour
{
    public Hand hand; // Hand 스크립트 참조
    public XRRayInteractor rayInteractor; // Ray Interactor 참조
    public Animator anim; //총 메인 애니메이션
    public ParticleSystem muzzle; //총 불꽃
    public int gunDamage; //총 데미지

    // 현재 총에 부착된 탄창
    public GameObject magazine;

    public InputActionProperty reloadButton;
    public GameObject Ghostmagazine; //탄창이 없을 때 보여줄 투명 탄창 

    // 탄창이 들어갈 소켓 인터랙터
    public XRSocketInteractor magazineSocket;

    public float rpm;
    private float interval;

    private void Awake()
    {
        if (false == muzzle) GetComponentInChildren<ParticleSystem>();
        if (false == anim) GetComponent<Animator>();
        float rps = rpm / 60f;
        interval = 1 / rps;

        if (!hand) hand = GetComponentInParent<Hand>();
        if (!rayInteractor) rayInteractor = GetComponentInParent<XRRayInteractor>();

        reloadButton.EnableDirectAction();
    }

    private void OnEnable()
    {
        reloadButton.action.performed += OnReloadInput;

        // 소켓 이벤트 연결: 탄창이 들어오거나 나갈 때 함수 호출
        if (magazineSocket != null)
        {
            magazineSocket.selectEntered.AddListener(OnMagazinePlaced);
            magazineSocket.selectExited.AddListener(OnMagazineRemoved);
        }
    }

    private void OnDisable()
    {
        reloadButton.action.performed -= OnReloadInput;

        // 소켓 이벤트 연결 해제
        if (magazineSocket != null)
        {
            magazineSocket.selectEntered.RemoveListener(OnMagazinePlaced);
            magazineSocket.selectExited.RemoveListener(OnMagazineRemoved);
        }
    }

    private void OnDestroy()
    {
        reloadButton.DisableDirectAction();
    }

    void OnReloadInput(InputAction.CallbackContext context)
    {
        ReLoad();
        Debug.Log("리로드를 눌렀습니다 ");
    }
    private float fireTime;

    private void Update()
    {
        if (!hand.isFire) return;

        if (Time.time < fireTime + interval)
        {
            return;
        }

        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                fireTime = Time.time;
                Debug.Log("Enemy를 맞췄습니다!");
                hit.collider.GetComponent<Enemy>().TakeDamage(gunDamage);
            }
        }
        Fire();
    }

    private void Fire()
    {
        muzzle.Play();
        anim.SetBool("Shoot", true);

        StopAllCoroutines();
        StartCoroutine(ResetShootAnimation());
    }

    private IEnumerator ResetShootAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Shoot", false);
    }

    public void PullTrigger(bool isOn)
    {
        if (!isOn)
        {
            muzzle.Stop();
        }
    }

    // 소켓에 탄창이 들어왔을 때 실행
    private void OnMagazinePlaced(SelectEnterEventArgs args)
    {
        // 소켓에 들어온 오브젝트를 magazine 변수에 할당
        magazine = args.interactableObject.transform.gameObject;
        Debug.Log("탄창이 장착되었습니다.");

        if (Ghostmagazine != null)
        {
            Ghostmagazine.SetActive(false);
        }


    }

    // 소켓에서 탄창이 나갔을 때 실행
    private void OnMagazineRemoved(SelectExitEventArgs args)
    {
        magazine = null;
        if (Ghostmagazine != null)
        {
            Ghostmagazine.SetActive(true);
        }
    }

    public void ReLoad()
    {
        // 소켓에 현재 탄창이 없으면 함수 종료
        if (!magazineSocket.hasSelection || magazineSocket.interactablesSelected.Count == 0)
        {
            Debug.Log("소켓에 탄창이 없습니다.");
            return;
        }

        // 소켓에서 탄창을 가져와 변수에 저장
        IXRSelectInteractable interactable = magazineSocket.interactablesSelected[0];
        GameObject droppedMagazine = interactable.transform.gameObject;
        Debug.Log("탄창변수 저장");

        // 소켓에서 탄창을 강제로 분리 (더 안전한 방법)
        magazineSocket.interactionManager.SelectExit(magazineSocket, interactable);
       
        XRGrabInteractable dropMagazineLayer = droppedMagazine.GetComponent<XRGrabInteractable>();
        dropMagazineLayer.interactionLayers = InteractionLayerMask.GetMask("SpentMagazine");

        // 물리 효과 적용
        Rigidbody rb = droppedMagazine.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        Destroy(droppedMagazine, 5f);
        magazine = null;

        if (Ghostmagazine != null)
        {
            Ghostmagazine.SetActive(true);
        }
    }

}