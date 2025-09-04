using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}

    public int ammoCount; //현재 탄약 카운트
    public int ammoMax; //탄약 최대치 
    public int score = 0; //스코어
    public int playercurrenthp; //플레이어 hp
    public int playermaxhp; //플레이어 최대 hp 
    public int rountCount; // 라운드 카운트 
    public List<GameObject> enemies = new List<GameObject>(); //적을 스포하여 관리할 리스트 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ammoCount = ammoMax; //초기 탄약 
        playercurrenthp = playermaxhp; //플레이어 hp 초기화 
    }
    
    
}
