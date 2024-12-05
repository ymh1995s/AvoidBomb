using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static PlayerState;


public abstract class BasePlayer : ROOTOBJECT
{
    protected int hp,maxHp;
    protected int state = (int)ActionState.Idle;
    private Vector3 destinationPos;

    // 1205테스트 기준 시작 값 30,15
    const int startPosX = 30;
    const int startPosY = 15;
    public int currentPosX { get; private set; } = startPosX;
    public int currentPosY { get; private set; } = startPosY;

    // 리스너 패턴! 그런데 Action을 사용하여 더 짧아진
    public event Action<int, int> HpUpdateTrigger;

    // 하위 스크립트
    public HPBar hpBar;
    public TileManager tilemanager;
    
    // 화염 도트딜
    Coroutine FireCoroutine = null;
    int remainedBurningTime = 0;


    protected virtual void Start()
    {
        // 구독
        HpUpdateTrigger += hpBar.UpdateHp;
    }

    protected virtual void Update()
    {
        if (state == (int)ActionState.Die) return;
        Move();
        CheckFireTIle();
        CheckGoal();
    }

    public virtual void Move()
    {
        // Idle(움직임기 가능함) => Move
        if(state == (int)ActionState.Idle)
        {
            Vector3 tempDestinationPos = transform.position;
            if (Input.GetKey(KeyCode.W))
            {
                tempDestinationPos = tilemanager.tilesInfo[currentPosX, ++currentPosY].transform.position;
                state = (int)ActionState.forward;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                tempDestinationPos = tilemanager.tilesInfo[currentPosX, --currentPosY].transform.position;
                state = (int)ActionState.back;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                tempDestinationPos = tilemanager.tilesInfo[--currentPosX, currentPosY].transform.position;
                state = (int)ActionState.left;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                tempDestinationPos = tilemanager.tilesInfo[++currentPosX, currentPosY].transform.position;
                state = (int)ActionState.right;
            }

            // 임의로 정한 보더라 나중에 하드 코딩 제거
            if (tempDestinationPos.z > 100 || tempDestinationPos.z < 0 || tempDestinationPos.x > 15 || tempDestinationPos.x < -15)
            {
                state= (int)ActionState.Idle;
                print("OUTBOARDER");
            }
            else
            {
                destinationPos = tempDestinationPos;
            }
        }
        // 이미 움직이는중인데 목적지 도착? Idle : Notrhing
        else if(state == (int)ActionState.forward || state == (int)ActionState.left || state == (int)ActionState.right || state == (int)ActionState.back)
        {
            Vector3 direction = (destinationPos - transform.position).normalized;
            transform.position += direction * 1f * Time.deltaTime;

            if ((transform.position - destinationPos ).magnitude < 0.1f)
            {
                transform.position = destinationPos;
                state = (int)ActionState.Idle;
            }
        }
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
    }

    protected virtual void Death()
    {
        //TODO
        state = (int)ActionState.Die;
        print($"DEATH");
    }

    public virtual void CheckGoal()
    {
        if (transform.position.z > 100)
        {
            Debug.Log("GOAL");
        }
    }

    protected virtual void CheckFireTIle()
    {
        Debug.DrawRay(transform.position , Vector3.down, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(transform.position , Vector3.down, out hit, 10))
        {
            Tile tile = hit.transform.GetComponent<Tile>();
            if(tile.state == Tile.State.Fire)
            {
                remainedBurningTime = 5;
                if(FireCoroutine == null)
                    FireCoroutine = StartCoroutine(Bunrning());
            }
        }
    }

    IEnumerator Bunrning()
    {
        while(remainedBurningTime>0)
        {
            hp--; // 딜 조정 인터페이스를 열어둘 필요는 있음
            remainedBurningTime--;
            HpUpdateTrigger?.Invoke(hp, maxHp);
            Debug.Log($"{remainedBurningTime} {hp} time/hp remained");
            yield return new WaitForSeconds(1f);
        }
        FireCoroutine = null;
    }

    public abstract void Skill();
}
