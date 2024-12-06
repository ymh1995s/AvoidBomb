using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectile : ROOTOBJECT
{
    [SerializeField] protected Vector3 targetPos;
    protected int dropSpeed;
    public int damage { get; set; }
    protected int fireRange;
    protected int fireDuration;

    private Vector3 shootStartPos;   // 발사대 위치
    public float moveDuration = 3f;  // 이동 시간 (3초)
    private float timeElapsed = 0f;  // 경과 시간

    public TileManager tileManager;

    private int maxRangeX;
    private int maxRangeY;

    protected enum MasterDamage
    {
        Bomb = 0,
        Missile = 40
    }

    protected virtual void Start()
    {
        shootStartPos = transform.position;
        tileManager = GameManager.Instance.tileManager;

        maxRangeX = tileManager.xoffsetEnd - tileManager.xoffsetStart;
        maxRangeY = tileManager.yoffsetEnd;

        targetPos = tileManager.tilesInfo[Random.Range(0, maxRangeX), Random.Range(0, maxRangeY)].transform.position;
        //targetPos = GameManager.Instance.player.transform.position; // 플레이어 타겟 테스트
    }

    protected virtual void Update()
    {
        Move();
    }

    //public void PlayerHit()
    //{
    //    GameManager.Instance.player.Hit(damage);
    //}

    // from GPT
    private void Move()
    {
        // 경과 시간 계산
        timeElapsed += Time.deltaTime;

        // 이동 중일 때만 진행
        if (timeElapsed < moveDuration)
        {
            // Lerp로 위치 보간 (linear interpolation)
            transform.position = Vector3.Lerp(shootStartPos, targetPos, timeElapsed / moveDuration);
        }
        else
        {
            // 3초가 지나면 목표 위치로 정확히 도달
            transform.position = targetPos;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag=="Obstacle")
        {
            Destroy(gameObject);
        }
    }
}
