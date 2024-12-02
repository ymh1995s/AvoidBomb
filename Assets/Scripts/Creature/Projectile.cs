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

    private Vector3 startPos;   // ���� ��ġ
    public float moveDuration = 3f;  // �̵� �ð� (3��)
    private float timeElapsed = 0f;  // ��� �ð�

    // ������ ��ư => ����
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
        // ��� �ð� ���
        timeElapsed += Time.deltaTime;

        // �̵� ���� ���� ����
        if (timeElapsed < moveDuration)
        {
            // Lerp�� ��ġ ���� (linear interpolation)
            transform.position = Vector3.Lerp(startPos, targetPos, timeElapsed / moveDuration);
        }
        else
        {
            // 3�ʰ� ������ ��ǥ ��ġ�� ��Ȯ�� ����
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
