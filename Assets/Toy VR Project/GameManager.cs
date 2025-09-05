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
    
    public int roundCount; 
    // EnemySpawn 스크립트 참조 추가
    public EnemySpawn enemySpawner;

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
    
    private void Start()
    {
        // 게임 시작 시 첫 라운드 시작
        StartNewRound();
    }
    
    // 새 라운드를 시작하는 함수
    public void StartNewRound()
    {
        roundCount++;
        Debug.Log($"라운드 {roundCount} 시작!");
        enemies.Clear(); // 새로운 라운드 시작 전 리스트 초기화
        enemySpawner.SpawnEnemies(); // EnemySpawn에 적 소환 명령
    }

    // 적이 죽었을 때 호출되어 리스트에서 적을 제거하는 함수
    public void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Debug.Log($"적 파괴! 남은 적 수: {enemies.Count}");

            // 모든 적을 처치하면 다음 라운드 시작
            if (enemies.Count == 0)
            {
                Debug.Log("모든 적을 처치했습니다! 다음 라운드 시작.");
                StartNewRound();
            }
        }
    }

    // 적이 스폰될 때 리스트에 추가하는 함수
    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }
}
