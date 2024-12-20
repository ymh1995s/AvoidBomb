using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static Cinemachine.CinemachineTargetGroup;
using static UnityEngine.GraphicsBuffer;

public class Projectile : ROOTOBJECT
{
    [SerializeField] protected Vector3 targetPos;
    protected int dropSpeed;
    public int damage { get; set; }
    protected int fireRange;
    protected int fireDuration;

    private Vector3 shootStartPos;   // 발사대 위치
    public float moveDuration = 10f;  // 이동 시간 (3초)
    private float timeElapsed = 0f;  // 경과 시간

    public TileManager tileManager;

    private int maxRangeX;
    private int maxRangeY;

    protected enum MasterDamage
    {
        Bomb = 5,
        Missile = 10
    }

    protected virtual void Start()
    {
        shootStartPos = transform.position;
        tileManager = GameManager.Instance.tileManager;

        maxRangeX = tileManager.xoffsetEnd - tileManager.xoffsetStart;
        maxRangeY = tileManager.yoffsetEnd;

        // 일정 확률로 플레이어 타게팅
        int quarter = Random.Range(0, 5);
        if(quarter ==0)
        {
            int playerPosX = GameManager.Instance.player.currentPosX + Random.Range(-2, 2);
            int playerPosY = GameManager.Instance.player.currentPosY + Random.Range(-2, 2);
            if (playerPosX < 0 || playerPosY < 0 || 
                playerPosX >= GameManager.Instance.tileManager.xoffsetEnd - GameManager.Instance.tileManager.xoffsetStart - 1 || playerPosY >= GameManager.Instance.tileManager.yoffsetEnd - 1)
            {
                targetPos = tileManager.tilesInfo[Random.Range(0, maxRangeX), Random.Range(0, maxRangeY)].transform.position;
            }
            else
            {
                targetPos = tileManager.tilesInfo[playerPosX, playerPosY].transform.position;
            }
        }
        else
        {
            targetPos = tileManager.tilesInfo[Random.Range(0, maxRangeX), Random.Range(0, maxRangeY)].transform.position;
        }

        // 타겟 방향으로 투사체 Rotation
        Vector3 direction = targetPos - transform.position; 
        Quaternion lookRotation = Quaternion.LookRotation(direction); 
        transform.rotation = lookRotation;
    }

    protected virtual void Update()
    {
       // LateUpdate로 뺌
    }

    private void LateUpdate()
    {
        Move();
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
            transform.position = Vector3.Lerp(shootStartPos, targetPos, timeElapsed / moveDuration);
        }
        else
        {
            // 3초가 지나면 목표 위치로 정확히 도달
            transform.position = targetPos;
        }
    }
}
