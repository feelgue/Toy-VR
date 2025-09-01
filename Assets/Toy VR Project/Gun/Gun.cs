using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Animator anim;
    public ParticleSystem muzzle;

    public float rpm; //분당 발사 횟수 
    private float interval; //발사 간 시간 간격

    private void Awake()
    {
        if (false == muzzle) GetComponentInChildren<ParticleSystem>();
        if (false == anim) GetComponent<Animator>();
        float rps = rpm / 60f; //초당 발사 횟수 
        interval = 1/rps;
        
    }

    private float fireTime; // 직전에 발사가 호출된 시간  
    private bool isTriggerOn;
    private void Update()
    {
        if(!isTriggerOn) return;
        if(Time.time > fireTime + interval){  return;} // 아직 다음 발사 되지 않았으면 return

        fireTime = Time.time;
        Fire();
    }

    private void Fire()
    {
        //총알 발사 
    }

    public void PullTrigger(bool isOn)
    {
        isTriggerOn = isOn;
        if (isOn)
        {
            muzzle.Play();
            anim.SetBool("Shoot",true);
        }
        else
        {
            muzzle.Stop();
            anim.SetBool("Shoot",false);
        }
    }
}
