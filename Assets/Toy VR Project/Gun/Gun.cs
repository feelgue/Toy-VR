using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Gun : MonoBehaviour
{
    public Hand hand; // Hand 스크립트 참조
    public XRRayInteractor rayInteractor; // Ray Interactor 참조
    public Animator anim; //총 메인 애니메이션
    public ParticleSystem muzzle; //총 불꽃
    public int gunDamage; //총 데미지

    public float rpm;
    private float interval;

    private void Awake()
    {
        if (false == muzzle) GetComponentInChildren<ParticleSystem>();
        if (false == anim) GetComponent<Animator>();
        float rps = rpm / 60f;
        interval = 1 / rps;

        // Hand와 Ray Interactor가 없다면 Hierarchy에서 찾아서 할당
        if (!hand) hand = GetComponentInParent<Hand>();
        if (!rayInteractor) rayInteractor = GetComponentInParent<XRRayInteractor>();
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
                Fire();
                Debug.Log("Enemy를 맞췄습니다!");
                hit.collider.GetComponent<Enemy>().TakeDamage(gunDamage);
            }
        }
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
}