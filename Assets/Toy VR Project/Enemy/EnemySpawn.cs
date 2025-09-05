using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public int enemyCount; //적을 소환할 수 
    public GameObject Enemyprefab; //소환할 적의 프리펩 
    public Transform[]  spawnpoints; //소환할 배열 
    
    // 적을 소환하는 함수
    public void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            // 스폰 포인트 배열의 인덱스를 벗어나지 않도록 나머지 연산자(%) 사용
            int spawnPointIndex = i % spawnpoints.Length;
            Transform spawnPoint = spawnpoints[spawnPointIndex];

            // 적 생성
            GameObject newEnemy = Instantiate(Enemyprefab, spawnPoint.position, spawnPoint.rotation);
            
            // GameManager 리스트에 추가
            GameManager.instance.AddEnemy(newEnemy);
        }
    }
}
