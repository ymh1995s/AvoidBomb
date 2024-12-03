using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BasePlayer : ROOTOBJECT
{
    protected int hp;
    protected int state;
    private Vector3 destinationPos;

    // 리스너 패턴! 그런데 Action을 사용하여 더 짧아진
    public event Action<float> HpUpdateTrigger;

    public HPBar hpBar;

    protected enum PlayerState
    {
        Idle,
        forward,
        left,
        right,
        back,
        skill
    }

    protected enum FireState
    {
        Normal,
        Burning
    }

    protected enum MasterHP
    {
        Basic = 100,
        FireFIghter = 150
    }

    protected virtual void Start()
    {
        state = (int)PlayerState.Idle;
        HpUpdateTrigger += hpBar.UpdateHp;
    }

    protected virtual void Update()
    {
        Move();
        CheckGoal();
        IsBurned();
    }

    public virtual void Move()
    {
        if(state == (int)PlayerState.Idle)
        {
            Vector3 tempDestinationPos = transform.position;
            if (Input.GetKey(KeyCode.W))
            {
                tempDestinationPos += new Vector3(0, 0, 1);
                state = (int)PlayerState.forward;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                tempDestinationPos += new Vector3(0, 0, -1);
                state = (int)PlayerState.back;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                tempDestinationPos += new Vector3(-1, 0, 0);
                state = (int)PlayerState.left;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                tempDestinationPos += new Vector3(1, 0, 0);
                state = (int)PlayerState.right;
            }

            if (tempDestinationPos.z > 100 || tempDestinationPos.z < 0 || tempDestinationPos.x > 15 || tempDestinationPos.x < -15)
            {
                state= (int)PlayerState.Idle;
                print("OUTBOARDER");
            }
            else
            {
                destinationPos = tempDestinationPos;
            }
        }
        else if(state == (int)PlayerState.forward || state == (int)PlayerState.left || state == (int)PlayerState.right || state == (int)PlayerState.back)
        {
            Vector3 direction = (destinationPos - transform.position).normalized;
            transform.position += direction * 1f * Time.deltaTime;

            if ((transform.position - destinationPos ).magnitude < 0.1f)
            {
                transform.position = destinationPos;
                state = (int)PlayerState.Idle;
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

    public virtual void Reborn()
    {
        // is this Necessary?
    }

    public virtual void CheckGoal()
    {
        if (transform.position.z > 100)
        {
            Debug.Log("GAOL");
        }
    }

    protected virtual void IsBurned()
    {
        int xoffsetStart = -30;
        int yoffsetStart = -15;
        if (GameManager.Instance.tileManager.tilesInfo[(int)transform.position.x- xoffsetStart, (int)transform.position.z].state- yoffsetStart == Tile.State.Fire)
        {
            print("았뜨거뜨거");
        }
    }

    IEnumerator Bunrning()
    {
        yield return new WaitForSeconds(5f);
    }

    public abstract void Skill();
}
