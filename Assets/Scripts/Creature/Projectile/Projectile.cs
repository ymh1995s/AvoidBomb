using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectile : ROOTOBJECT
{
    [SerializeField] protected Vector3 targetPos;
    protected int dropSpeed;
    protected int damage;
    protected int fireRange;
    protected int fireDuration;

    private Vector3 startPos;   // 시작 위치
    public float moveDuration = 3f;  // 이동 시간 (3초)
    private float timeElapsed = 0f;  // 경과 시간
    
    protected enum MasterDamage
    {
        Bomb = 10,
        Missile = 40
    }

    protected virtual void Start()
    {
        startPos = transform.position;
        //targetPos = new Vector3(Random.Range(-15f, 15f), 0f, Random.Range(10f, 30f));
        targetPos = GameManager.Instance.player.transform.position;
    }

    protected virtual void Update()
    {
        Move();
    }

    public void PlayerHit()
    {
        GameManager.Instance.player.Hit(damage);
    }

    // from GPT
    private void Move()
    {
        // 경과 시간 계산
        timeElapsed += Time.deltaTime;

        // 이동 중일 때만 진행
        if (timeElapsed < moveDuration)
        {
            // Lerp로 위치 보간 (linear interpolation)
            transform.position = Vector3.Lerp(startPos, targetPos, timeElapsed / moveDuration);
        }
        else
        {
            // 3초가 지나면 목표 위치로 정확히 도달
            transform.position = targetPos;
        }
    }
}
