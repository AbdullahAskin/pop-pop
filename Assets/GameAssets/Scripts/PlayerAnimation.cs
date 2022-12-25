using UnityEngine;
using Spine.Unity;
using System;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private static readonly int PrepForJump = Animator.StringToHash("trigger");
    private static readonly int SpinStatus = Animator.StringToHash("spinStatus");
    private static readonly int Spin = Animator.StringToHash("spin");

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void TogglePrepForJump(bool state)
    {
        animator.SetBool(PrepForJump, state);
    }

    public void UpdatePrepForJump(float normalizedInputRate)
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.shortNameHash != PrepForJump)
        {
            TogglePrepForJump(true);
            return;
        }
        
        var currentRatio = Mathf.Lerp(stateInfo.normalizedTime, normalizedInputRate, .15f);
        animator.Play(stateInfo.shortNameHash, 0, currentRatio);
    }

    public void ToggleSpin(bool status)
    {
        animator.SetBool(SpinStatus, status);
        
        if (status)
        {
            animator.SetTrigger(Spin);    
        }
    }
}
