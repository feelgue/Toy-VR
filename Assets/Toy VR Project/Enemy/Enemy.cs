using UnityEngine;
// using Lean.Pool; // LeanPool 사용을 위해 추가

public class Enemy : MonoBehaviour
{
    public int currenthp; //적 현재 체력
    public int maxHp = 100; //적 최대 체력
    public int damage; //적 총알  데미지 
    public Animator anim;

    // --- 총알 발사를 위한 변수 추가 ---
    public GameObject bulletPrefab; // 발사할 총알 프리팹
    public Transform firePoint;     // 총알이 생성될 위치 (적의 총구)
    public float bulletSpeed = 20f; // 총알 속도
    public float fireRate = 2f;     // 초당 발사 횟수 (예: 2초에 한 번)
    private float nextFireTime;     // 다음 발사 가능한 시간
    private bool isDead;            //적의 죽음 판단 
    private Transform playerTransform; // 플레이어의 위치 정보

    private void Awake()
    {
        currenthp = maxHp;
        anim = GetComponent<Animator>();
        isDead = false;
    }

    private void Start()
    {
        // "Player" 태그를 가진 오브젝트를 찾아서 할당
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        
    }

    private void Update()
    {
        // 플레이어 트랜스폼이 존재하고, 다음 발사 시간이 되었는지 확인
        if (playerTransform != null && Time.time >= nextFireTime && !isDead)
        {
            // 발사 함수 호출
            FireBullet();
            // 다음 발사 시간 갱신
            nextFireTime = Time.time + 1f / fireRate;
        }
    }
    
    // --- 총알 발사 함수 추가 ---
    private void FireBullet()
    { 
        //적의 사격 오차
        Vector3 shooterror = new Vector3(Random.Range(0, 0.2f), Random.Range(0, 0.2f), Random.Range(0, 0.2f)); 
        // 플레이어를 바라보는 방향을 계산
        Vector3 direction = ((playerTransform.position + shooterror) - firePoint.position).normalized;
        Vector3 Enemydirection = (playerTransform.position - firePoint.position).normalized;
        
        // 방향을 회전 값으로 변환
        Quaternion bulletRotation = Quaternion.LookRotation(direction);
        
        //적이 방향을 플레이어로 변환
        gameObject.transform.rotation = Quaternion.LookRotation(Enemydirection);

        // 총알 프리팹을 firePoint 위치와, 플레이어를 바라보는 회전 값으로 생성
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        
        // 생성된 총알에 'Enemy' 스크립트의 레퍼런스를 전달
        bullet.GetComponent<EnemyProjectile>().enemy = this;
        anim.SetTrigger("Fire");
    }
    
    public void TakeDamage(int damage)
    {
        if (isDead == true){return;}

        currenthp -=  damage;
        anim.SetTrigger("Hit");
        Debug.Log($"현재 체력 {currenthp}");
        if (currenthp <= 0)
        {
            
            Death();
        }
    }

    void Death()
    {
        GameManager.instance.score += 10;
        isDead = true;
        anim.SetTrigger("Die");
        Destroy(gameObject,3f);
        
        // 적이 파괴되기 직전에 GameManager에 알림
        GameManager.instance.RemoveEnemy(gameObject);
    }
}