using Unity.VisualScripting;
using UnityEngine;
using static PlayerState;

public class BasicPlayer : BasePlayer
{
    // Update is called once per frame
    private Animator animator;
    int preAnimState = 0;

    protected override void Start()
    {
        base.Start();
        hp = (int)MasterHP.Basic;
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        CheckAnimState();
    }

    // 직업마다 애니메이션이 다를 수 있으니까 여기에 놨음
    // 루키스 방식(3강 유니티)으로 더티플래그 형식으로 빼면 부하를 줄일 수 있긴 할거같은데
    void CheckAnimState()
    {
        if(preAnimState!= state)
        {
            if (state == (int)ActionState.forward)
            {
                animator.SetTrigger("RunForward");
            }
            else if (state == (int)ActionState.back)
            {
                animator.SetTrigger("RunBack");
            }
            else if (state == (int)ActionState.right)
            {
                animator.SetTrigger("RunRight");
            }
            else if (state == (int)ActionState.left)
            {
                animator.SetTrigger("RunLeft");
            }
            else if (state == (int)ActionState.Idle)
            {
                animator.SetTrigger("Idle");
            }
            preAnimState = state;
        }
    }

    public override void Skill()
    {
        // TODO SKILL
    }

}