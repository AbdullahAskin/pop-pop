using System;
using Lean.Touch;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float minInputRateForAimAssistance;
    [SerializeField] private float maximumForceCap;
    
    private PlayerAnimation _playerAnimation;

    private void Start()
    {
        _playerAnimation = GameManager.Instance.currentPlayer.animation;
        
        ToggleInput(true);
    }
    
    public void ToggleInput(bool bind)
    {
        if (bind)
        {
            LeanTouch.OnFingerDown += OnFingerDown;
            LeanTouch.OnFingerUp += OnFingerUp;
            LeanTouch.OnFingerUpdate += OnFingerUpdate;
        }
        else
        {
            LeanTouch.OnFingerDown -= OnFingerDown;
            LeanTouch.OnFingerUp -= OnFingerUp;
            LeanTouch.OnFingerUpdate -= OnFingerUpdate;
        }
    }
    
    private void OnFingerDown(LeanFinger finger)
    {  
        AimManager.Instance.ToggleIndicators(true);
    }
    
    private void OnFingerUp(LeanFinger finger)
    {   
        AimManager.Instance.ToggleIndicators(false);
        _playerAnimation.TogglePrepForJump(false);
    }
    
    private void OnFingerUpdate(LeanFinger finger)
    {
        var fingerDeltaPos = finger.ScreenPosition - finger.StartScreenPosition;
        var inputRate = Mathf.Clamp(fingerDeltaPos.magnitude / (Screen.width * maximumForceCap), 0, 1);
        if (inputRate < minInputRateForAimAssistance) return;

        var normalizedInputRate = (inputRate - minInputRateForAimAssistance) / (1f - minInputRateForAimAssistance);

        AimManager.Instance.StepAimGuide(fingerDeltaPos.normalized, normalizedInputRate);
        GameManager.Instance.currentPlayer.animation.UpdatePrepForJump(normalizedInputRate);
    }
}
