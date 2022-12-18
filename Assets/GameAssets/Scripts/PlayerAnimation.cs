using UnityEngine;
using Spine.Unity;
using System;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    [SpineAnimation]
    [SerializeField] private string prepareForJumpAnimation;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void TogglePrepForJump(bool state)
    {
        animator.SetBool(prepareForJumpAnimation, state);
    }


    public void UpdatePrepForJump(float ratio)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.shortNameHash != Animator.StringToHash(prepareForJumpAnimation)) return;

        var lerpedValue = Mathf.Lerp(stateInfo.normalizedTime, ratio, .15f);
        animator.Play(stateInfo.shortNameHash, 0, lerpedValue);
    }
}
