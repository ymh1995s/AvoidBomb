using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BasePlayer : ROOTOBJECT
{
    protected int state;
    private Vector3 destinationPos;

    protected enum State
    {
        Idle,
        forward,
        left,
        right,
        back,
        skill
    }

    protected virtual void Start()
    {
        state = (int)State.Idle;
    }

    protected virtual void Update()
    {
        Move();
        CheckGoal();
    }

    public virtual void Move()
    {
        if(state == (int)State.Idle)
        {
            Vector3 tempDestinationPos = transform.position;
            if (Input.GetKey(KeyCode.W))
            {
                tempDestinationPos += new Vector3(0, 0, 1);
                state = (int)State.forward;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                tempDestinationPos += new Vector3(0, 0, -1);
                state = (int)State.back;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                tempDestinationPos += new Vector3(-1, 0, 0);
                state = (int)State.left;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                tempDestinationPos += new Vector3(1, 0, 0);
                state = (int)State.right;
            }

            if (tempDestinationPos.z > 100 || tempDestinationPos.z < 0 || tempDestinationPos.x > 15 || tempDestinationPos.x < -15)
            {
                state= (int)State.Idle;
                print("OUTBOARDER");
            }
            else
            {
                destinationPos = tempDestinationPos;
            }
        }
        else if(state == (int)State.forward || state == (int)State.left || state == (int)State.right || state == (int)State.back)
        {
            Vector3 direction = (destinationPos - transform.position).normalized;
            transform.position += direction * 1f * Time.deltaTime;

            if ((transform.position - destinationPos ).magnitude < 0.1f)
            {
                transform.position = destinationPos;
                state = (int)State.Idle;
            }
        }
    }

    public virtual void Hit(int damage)
    {

    }
    public virtual void Death()
    {

    }
    public virtual void Reborn()
    {

    }
    public virtual void CheckGoal()
    {
        if (transform.position.z > 100)
        {
            Debug.Log("GAOL");
        }
    }


    public abstract void Skill();
}
