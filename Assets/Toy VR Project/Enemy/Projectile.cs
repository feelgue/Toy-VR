using System;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f; // 발사 속도
    public float lifetime = 3f; // 발사체 생존 시간
    public Enemy enemy;
    void Start()
    {
        // 발사체를 일정 시간 후 자동으로 파괴
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // 발사체를 앞 방향(자신의 Transform.forward)으로 이동
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트의 태그를 확인
        if (other.CompareTag("Player"))
        {
            // 충돌한 오브젝트의 최상위 부모에서 Player 스크립트 참조
            Player player = other.transform.root.GetComponent<Player>();
        
            // Player 스크립트가 있는지 확인
            if (player != null)
            {
                player.takeDamage(enemy.damage);
                Destroy(gameObject);
                Debug.Log($"데미지를 {enemy.damage} 입혔습니다");
            }
        }
    }
}