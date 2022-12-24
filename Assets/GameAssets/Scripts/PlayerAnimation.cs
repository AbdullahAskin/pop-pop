using UnityEngine;
using Spine.Unity;
using System;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;

    [SpineAnimation]
    [SerializeField] private string prepareForJumpAnimation;

    private static readonly int Spin = Animator.StringToHash("spin");

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void TogglePrepForJump(bool status)
    {
        _animator.SetBool(prepareForJumpAnimation, status);
    }

    public void UpdatePrepForJump(float ratio)
    {
        var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.shortNameHash != Animator.StringToHash(prepareForJumpAnimation)) return;

        var lerpedValue = Mathf.Lerp(stateInfo.normalizedTime, ratio, .15f);
        _animator.Play(stateInfo.shortNameHash, 0, lerpedValue);
    }

    public void TriggerSpin()
    {
        _animator.SetTrigger(Spin);
    }
}
