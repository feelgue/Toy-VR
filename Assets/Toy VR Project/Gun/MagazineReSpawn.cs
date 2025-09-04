using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineReSpawn : MonoBehaviour
{
    public GameObject MagazinePrefab; //복사할 오브젝트 
    public Transform MagazineSpawnPoint; // 소환지점 
    public int magazinecount; // 10개 넘지않게하기 위해 
    public int maxMagazines = 10; 

    
    private void OnTriggerExit(Collider other)
    {
        // Only spawn a new magazine if the collider leaving is the one we spawned
        if (other.CompareTag("Magazine") && magazinecount < maxMagazines)
        {
            Instantiate(MagazinePrefab, MagazineSpawnPoint.position, Quaternion.identity);
            magazinecount++; // Increment the count when a new one is spawned
            Debug.Log($"새 탄창이 생성되었습니다. 현재 탄창 수: {magazinecount}");
        }
    }

}