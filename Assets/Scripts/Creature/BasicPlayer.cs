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
        // Animator 컴포넌트 가져오기
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        CheckAnimState();

    }

    void CheckAnimState()
    {
        if(preAnimState!= state)
        {
            if (state == (int)State.forward)
            {
                animator.SetTrigger("RunForward");
            }
            else if (state == (int)State.back)
            {
                animator.SetTrigger("RunBack");
            }
            else if (state == (int)State.right)
            {
                animator.SetTrigger("RunRight");
            }
            else if (state == (int)State.left)
            {
                animator.SetTrigger("RunLeft");
            }
            else if (state == (int)State.Idle)
            {
                animator.SetTrigger("Idle");
            }
            preAnimState = state;
        }
    }

    public override void Skill()
    {
        throw new System.NotImplementedException();
    }

}
