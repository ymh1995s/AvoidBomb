using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayersEnumInfo;


public abstract class BasePlayer : ROOTOBJECT
{
    protected int hp,maxHp;
    protected int state = (int)ActionState.Idle;
    private Vector3 destinationPos;

    public int startPosX { get; private set; }
    public int startPosY { get; private set; }
    public int currentPosX { get; private set; }
    public int currentPosY { get; private set; }

    // 리스너 패턴! 그런데 Action을 사용하여 더 짧아진
    public event Action<int, int> HpUpdateTrigger;

    // 하위 스크립트
    public HPBar hpBar;
    public TileManager tilemanager;
    
    // 화염 도트딜
    Coroutine FireCoroutine = null;
    int remainedBurningTime = 0;

    // 하위 컴포넌트
    public Image hitEffect;
    public ParticleSystem fireEffect;
    AudioSource audio;

    protected virtual void Start()
    {
        // 구독
        HpUpdateTrigger += hpBar.UpdateHp;
        currentPosX = startPosX = tilemanager.xoffsetEnd;
        currentPosY = startPosY = tilemanager.yoffsetStart;

        // 컴포넌트
        audio = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {
        // LateUpdate로 뺌
    }

    protected virtual void LateUpdate()
    {
        if (state == (int)ActionState.Die) return;
        Move();
        CheckFireTIle();
    }

    public virtual void Move()
    {
        // Idle(움직임기 가능함) => Move
        if(state == (int)ActionState.Idle)
        {
            ValueTuple<int, int> ReservePos = (currentPosX, currentPosY);
            if (Input.GetKey(KeyCode.W))
            {
                ReservePos.Item2++;
                state = (int)ActionState.forward;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                ReservePos.Item2--;
                state = (int)ActionState.back;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                ReservePos.Item1--;
                state = (int)ActionState.left;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                ReservePos.Item1++;
                state = (int)ActionState.right;
            }

            if(CanGo(ReservePos))
            {
                currentPosX = ReservePos.Item1;
                currentPosY = ReservePos.Item2;
                destinationPos = tilemanager.tilesInfo[currentPosX, currentPosY].transform.position;
            }
            else
            {
                state = (int)ActionState.Idle;
            }
        }

        // 이미 움직이는중인데 목적지 도착? Idle : Nothing
        else if(state == (int)ActionState.forward || state == (int)ActionState.left || state == (int)ActionState.right || state == (int)ActionState.back)
        {
            Vector3 direction = (destinationPos - transform.position).normalized;
            transform.position += direction * 2f * Time.deltaTime;

            if ((transform.position - destinationPos ).magnitude < 0.2f)
            {
                transform.position = destinationPos;
                state = (int)ActionState.Idle;
            }
        }
    }

    bool CanGo(ValueTuple<int, int> value)
    {
        // 범위 체크
        if(value.Item1 >= (tilemanager.xoffsetEnd - tilemanager.xoffsetStart) || value.Item1 < 0
            || value.Item2>= tilemanager.yoffsetEnd || value.Item2<0)
        {
            Debug.Log("OUTBOARDER");
            return false;
        }

        // 장애물 체크
        if (tilemanager.tilesInfo[value.Item1, value.Item2].state == Tile.State.Obstacle)
        {
            Debug.Log("OBSTACLE");
            return false;
        }

        return true;
    }

    public virtual void Hit(int damage)
    {
        hp -= damage;
        //print($"{hp} remained");

        HpUpdateTrigger?.Invoke(hp, maxHp);

        if (hp <= 0)
        {
            Death();
        }
        else
        {
            audio.Play();
            StartCoroutine(CoHitEffect());
        }
    }

    protected virtual void Death()
    {
        state = (int)ActionState.Die;
        StartCoroutine(CoLoadDefeatPopup());
    }

    IEnumerator CoLoadDefeatPopup()
    {
        yield return new WaitForSeconds(3f);
        GameManager.Instance.LoadLosePopup();
    }

    protected virtual void CheckFireTIle()
    {
        // 아마 레이관련인거같은데 게임 시작 후 아직 움직이지 않았을 때 화염 감지 못함
        // 시작인 상태를 감안할 때 냅둬도 될 듯
        // 후순위 TODO로 나중에 픽스
        Debug.DrawRay(transform.position , Vector3.down, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(transform.position , Vector3.down, out hit, 10))
        {
            Tile tile = hit.transform.GetComponent<Tile>();
            if (tile == null) return;
            if(tile.state == Tile.State.Fire)
            {
                remainedBurningTime = 3;
                if(FireCoroutine == null)
                    FireCoroutine = StartCoroutine(CoBunrning());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO : 나중에는 Bomb와 Missile로 분기처리괴 될 수 있으니 레이어도 고려
        if (other.CompareTag("Projectile"))
        {
            Destroy(other.gameObject);
            Projectile projectile = other.GetComponent<Projectile>();
            Hit(projectile.damage);
        }
    }

    IEnumerator CoBunrning()
    {
        fireEffect.Play();
        while (remainedBurningTime>0)
        {
            //hp--; // 딜 조정 인터페이스를 열어둘 필요는 있음
            //HpUpdateTrigger?.Invoke(hp, maxHp);
            Hit((int)FireDoTDeal.Basic); // 다른 인터페이스로 열어둘 필요는 있음

            remainedBurningTime--;
            Debug.Log($"{remainedBurningTime} {hp} time/hp remained");
            yield return new WaitForSeconds(1f);
        }
        fireEffect.Stop();
        FireCoroutine = null;
    }

    IEnumerator CoHitEffect()
    {
        hitEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitEffect.gameObject.SetActive(false);
    }


    public abstract void Skill();
}
