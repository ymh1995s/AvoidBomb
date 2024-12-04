using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static PlayerState;

public abstract class BasePlayer : ROOTOBJECT
{
    protected int hp;
    protected int state;
    private Vector3 destinationPos;

    // 리스너 패턴! 그런데 Action을 사용하여 더 짧아진
    public event Action<float> HpUpdateTrigger;

    public HPBar hpBar;

    Coroutine FireCoroutine = null;
    int remainedBurningTime = 0;


    protected virtual void Start()
    {
        state = (int)ActionState.Idle;
        HpUpdateTrigger += hpBar.UpdateHp;
    }

    protected virtual void Update()
    {
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
                tempDestinationPos += new Vector3(0, 0, 1);
                state = (int)ActionState.forward;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                tempDestinationPos += new Vector3(0, 0, -1);
                state = (int)ActionState.back;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                tempDestinationPos += new Vector3(-1, 0, 0);
                state = (int)ActionState.left;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                tempDestinationPos += new Vector3(1, 0, 0);
                state = (int)ActionState.right;
            }

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

        float hpRetion = hp / (float)MasterHP.Basic;
        HpUpdateTrigger?.Invoke(hpRetion);

        if (hp <= 0)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        //TODO
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
            hp--;
            Debug.Log($"{remainedBurningTime} {hp} time/hp remained");
            remainedBurningTime--;
            yield return new WaitForSeconds(1f);
        }
        FireCoroutine = null;
    }

    public abstract void Skill();
}
