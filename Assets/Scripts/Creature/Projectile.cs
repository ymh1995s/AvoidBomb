using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectile : ROOTOBJECT
{
    protected Vector3 targetPos;
    protected int dropSpeed;
    protected int damage;
    protected int fireRange;
    protected int fireDuration;

    private Vector3 startPos;   // 시작 위치
    public float moveDuration = 3f;  // 이동 시간 (3초)
    private float timeElapsed = 0f;  // 경과 시간

    // 옵저버 패튼 => 보류
    //public delegate void DHitTrigger(Vector3 pos);
    //static public event DHitTrigger HitTrigger;

    private void Start()
    {
        startPos = transform.position;
        targetPos = new Vector3(Random.Range(-15f, 15f), 0f, Random.Range(10f, 30f));
    }

    private void Update()
    {
        Move();
    }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            //HitTrigger?.Invoke(targetPos);
            Destroy(gameObject);
        }
    }
}
