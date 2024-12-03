using Unity.VisualScripting;
using UnityEngine;

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
    void CheckAnimState()
    {
        if(preAnimState!= state)
        {
            if (state == (int)PlayerState.forward)
            {
                animator.SetTrigger("RunForward");
            }
            else if (state == (int)PlayerState.back)
            {
                animator.SetTrigger("RunBack");
            }
            else if (state == (int)PlayerState.right)
            {
                animator.SetTrigger("RunRight");
            }
            else if (state == (int)PlayerState.left)
            {
                animator.SetTrigger("RunLeft");
            }
            else if (state == (int)PlayerState.Idle)
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
