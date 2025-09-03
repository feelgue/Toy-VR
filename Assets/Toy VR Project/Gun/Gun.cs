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
    public GameObject magazine; //탄창 오브젝트 
    public InputActionProperty reloadButton;
    public GameObject Ghostmagazine; //탄창이 없을때 보여줄 투명 탄창 
    public Transform magzineoffset;  //탄창이 들어갈 transfrom 
    
    public float rpm; // 
    private float interval; //

    // // XR Socket Interactor를 참조
    // public XRSocketInteractor magazineSocket;

    private void Awake()
    {
        if (false == muzzle) GetComponentInChildren<ParticleSystem>();
        if (false == anim) GetComponent<Animator>();
        float rps = rpm / 60f;
        interval = 1 / rps;

        // Hand와 Ray Interactor가 없다면 Hierarchy에서 찾아서 할당
        if (!hand) hand = GetComponentInParent<Hand>();
        if (!rayInteractor) rayInteractor = GetComponentInParent<XRRayInteractor>();

        reloadButton.EnableDirectAction();
    }

    private void OnEnable()
    {
        reloadButton.action.performed += OnReloadInput;
        // //소켓 이벤트 연결 
        // if (magazineSocket != null)
        // {
        //     magazineSocket.selectEntered.AddListener(OnMagazinePlaced);
        //     magazineSocket.selectExited.AddListener(OnMagazineRemoved);
        // }
    }

    private void OnDisable()
    {
        reloadButton.action.performed -= OnReloadInput;
        // // 소켓 이벤트 연결 해제
        // if (magazineSocket != null)
        // {
        //     magazineSocket.selectEntered.RemoveListener(OnMagazinePlaced);
        //     magazineSocket.selectExited.RemoveListener(OnMagazineRemoved);
        // }
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
        // Hand 스크립트의 isFire 변수를 통해 트리거가 당겨졌는지 확인
        if (!hand.isFire) return;

        // 총알이 발사될 시간 간격이 지났는지 확인
        if (Time.time < fireTime + interval)
        {
            return;
        }
        
        // Raycast를 쏴서 닿은 물체가 있는지 확인
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            // Raycast에 맞은 물체의 태그가 "Enemy"인지 확인
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

        // 총을 쏜 후 잠시 대기했다가 애니메이션 상태를 false로
        StopAllCoroutines();
        StartCoroutine(ResetShootAnimation());
    }

    private IEnumerator ResetShootAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Shoot", false);
    }
    
    // 이전에 Hand 스크립트에서 호출했던 PullTrigger 함수는 이제 필요 없습니다.
    // 하지만 애니메이션과 머즐 플래시를 제어하기 위해 남겨둘 수 있습니다.
    public void PullTrigger(bool isOn)
    {
        if (!isOn)
        {
            muzzle.Stop();
        }
    }

    // --- 소켓 이벤트 핸들러 추가 ---
    // private void OnMagazinePlaced(SelectEnterEventArgs args)
    // {
    //     magazine = args.interactableObject.transform.gameObject;
    //     Debug.Log("탄창이 장착되었습니다.");
    //     
    //     // if (Ghostmagazine != null)
    //     // {
    //     //     Ghostmagazine.SetActive(false);
    //     // }
    // }

    // private void OnMagazineRemoved(SelectExitEventArgs args)
    // {
    //     // 탄창이 소켓에서 제거될 때 magazine 변수 초기화
    //     magazine = null;
    //     // if (Ghostmagazine != null)
    //     // {
    //     //     Ghostmagazine.SetActive(true);
    //     // }
    // }
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Magazine") && magazine == null)
    //     {
    //         magazine = other.gameObject;
    //         
    //         magazine.transform.SetParent(magzineoffset); //부모 만들어 주기 
    //         magazine.transform.localPosition = magzineoffset.transform.localPosition;
    //         magazine.transform.localRotation = Quaternion.identity;
    //         magazine.GetComponent<BoxCollider>().enabled= false; //다시 장착될수있기 때문에 collrider false 
    //         magazine.GetComponent<Rigidbody>().isKinematic = true; //탄창의 물리효과 헤제
    //         Ghostmagazine.gameObject.SetActive(false);
    //         
    //     }
    // }

    public void ReLoad()
    {
        if (magazine == null)
        {
            return;
        }
        // // 소켓에서 탄창 제거
        // magazineSocket.selectExited.Invoke(new SelectExitEventArgs()
        // {
        //     interactorObject = magazineSocket,
        //     interactableObject = magazineSocket.interactablesSelected[0]
        // });
        Ghostmagazine.gameObject.SetActive(true);
        magazine.transform.SetParent(null);
        magazine.GetComponent<BoxCollider>().enabled= false; //다시 장착될수있기 때문에 collrider false 
        magazine.GetComponent<Rigidbody>().isKinematic = false; //탄창 떨구기
        Destroy(magazine.gameObject , 5f); //5초후 삭제 
        magazine = null; //탄창 null 바꾸기 
    }
}