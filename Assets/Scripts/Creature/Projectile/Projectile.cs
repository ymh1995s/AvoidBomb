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
    private float timeElapsed = 0f;  // 경과 시간\

    public TileManager tileManager;
    private const int xoffsetStart = -15;
    private const int xoffsetEnd = 15;
    private const int yoffsetStart = -15;
    private const int yoffsetEnd = 20;

    protected enum MasterDamage
    {
        Bomb = 10,
        Missile = 40
    }

    protected virtual void Start()
    {
        startPos = transform.position;
        // 여기 레인지도 타일 값에 종속저이여야 하므로 후에 하드코딩 제거

        //int randomX = Random.Range(0, xoffsetEnd - xoffsetStart);
        //int randomY = Random.Range(0, yoffsetEnd - yoffsetStart);

        // TODO : 이것도 좌표 받아서 유효한 범위로 설정해야됨. 지금은 귀찮아서 대충 유효범위 하드코딩
        targetPos = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-15, 10f));
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag=="Obstacle")
        {
            Destroy(gameObject);
        }
    }
}
